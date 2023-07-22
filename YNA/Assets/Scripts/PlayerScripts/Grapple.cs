//*************************************************
// Project: We're Tethered Together
// File: Grapple.cs
// Author/s: Emmy Berg
//           Corbyn Lamar
//
// Desc: Rope controls & actions for player
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Grapple : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    GameObject partner;
    LineRenderer line;
    SpringJoint2D target;

    Vector2 playerLinePos;
    Vector2 partnerLinePos;

    GameObject topSolidMap;

    public float maxLimit = 0.95f;
    public float minLimit = 0.75f;
    public float breakingPoint = 7f;
    public float pullSpeed = 1f;
    public float reelSpeed = 1.75f;
    public float airReelSpeed = 1f;

    float currDist;
    float currLength;

    [HideInInspector]
    public bool isTethered;
    [HideInInspector]
    public bool canLatch;
    [HideInInspector]
    public bool isLatching;

    bool isExtending;
    bool isReeling;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        partner = GameObject.FindGameObjectWithTag("Partner");
        topSolidMap = GameObject.FindGameObjectWithTag("TopSolidMap");

        line = GetComponent<LineRenderer>();
        target = partner?.GetComponent<SpringJoint2D>();

        if (target != null)
        {
            Tethered(true);
        }
        else
        {
            line.enabled = false;
        }
   
        isExtending = false;
        currLength = maxLimit;
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        if (partner == null)
        {
            return;
        }

        SetRope();

        currDist = (transform.position - partner.transform.position).magnitude;

        if (currDist > breakingPoint)
        {
            Tethered(false);
            if (isLatching)
            {
                isLatching = false;
                partner.transform.Find("LatchRadius").GetComponent<Latch>().ReleaseObject();
            }
        }

        if (line.enabled && !isReeling)
        {
            Pull();
        }

        if (isExtending)
        {
            target.distance += 0.02f;
            currLength = target.distance;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // SET ROPE ============================================================
    void SetRope()
    {
        playerLinePos = new Vector2(transform.position.x, transform.position.y - 0.235f);
        partnerLinePos = new Vector2(partner.transform.position.x, partner.transform.position.y - 0.235f);

        line.SetPosition(0, playerLinePos);
        line.SetPosition(1, partnerLinePos);

        target.connectedAnchor = transform.position;
    }


    public void EnableRope(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!canLatch)
            {
                if (currDist <= (maxLimit + 1) && !target.enabled)
                {
                    Tethered(true);
                }
                else
                {
                    Tethered(false);
                }
            }

            else
            {
                if(isLatching)
                {
                    isLatching = false;
                    partner.transform.Find("LatchRadius").GetComponent<Latch>().ReleaseObject();
                }
                else
                {
                    isLatching = true;
                    partner.transform.Find("LatchRadius").GetComponent<Latch>().GrabObject();
                }
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // PULL ================================================================
    void Pull()
    {
        target.frequency = pullSpeed;

        if (isExtending == false)
        {
            if (currDist < minLimit)
            {
                target.distance = minLimit;
            }
            else if (currDist < currLength)
            {
                target.distance = currDist;
            }
            else
            {
                target.distance = currLength;
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // REEL IN =============================================================
    public void ReelIn(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isExtending = false;
            target.distance = 0;
            currLength = maxLimit;
            maxLimit = 0.95f;

            if (gameObject.GetComponent<PlayerController>() && gameObject.GetComponent<PlayerController>().IsGrounded() == false)
            {
                target.frequency = airReelSpeed;
            }
            else
            {
                target.frequency = reelSpeed;
            }

            isReeling = true;
        }
        else
        {
            isReeling = false;
        }

        // Logic for topsolid and partner
        if (topSolidMap)
        {
            topSolidMap.GetComponent<PlatformEffector2D>().colliderMask |= (1 << partner.layer);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // EXTEND ==============================================================
    public void Extend(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isExtending = true;
        }
        else
        {
            isExtending = false;
        }

        // Logic for topsolid and partner
        if (topSolidMap)
        {
            topSolidMap.GetComponent<PlatformEffector2D>().colliderMask &= ~(1 << partner.layer);
        }
    }

    void Tethered(bool tethered)
    {
        isTethered = tethered;
        target.enabled = tethered;
        line.enabled = tethered;
    }
}
