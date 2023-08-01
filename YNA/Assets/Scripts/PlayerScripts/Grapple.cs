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
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Grapple : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    GameObject topSolidMap;
    LineRenderer line;
    SpringJoint2D target;

    Vector2 playerLinePos;
    Vector2 partnerLinePos;

    public float maxTetherDist = 1.5f;
    public float minTetherDist = 0.35f;
    public float ropeSpeed = 3f;

    [HideInInspector]
    public float minRopeLimit = 0.25f;
    [HideInInspector]
    public float maxRopeLimit = 10f;
    float currMaxRopeLimit;

    float currDistFromPartner;
    float currRopeLength;

    [HideInInspector]
    public bool isTethered;

    bool isExtending;
    bool isReeling;

    public bool startTethered = true;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    private void Awake()
    {
        topSolidMap = GameObject.FindGameObjectWithTag("TopSolidMap");
        line = GetComponent<LineRenderer>();
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        target = Info.partner?.GetComponent<SpringJoint2D>();

        if (target != null && startTethered)
        {
            Tethered(true);
        }
        else
        {
            Tethered(false);
        }

        isExtending = false;
        currRopeLength = maxTetherDist;
        currMaxRopeLimit = currRopeLength;
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        if (Info.partner == null)
        {
            return;
        }

        SetRope();

        currDistFromPartner = (transform.position - Info.partner.transform.position).magnitude;

        if (isTethered)
        {
            Pull();
        }

        if (isReeling && currRopeLength > minRopeLimit)
        {
            target.distance -= (Time.deltaTime * ropeSpeed);
            currRopeLength = target.distance;
            currMaxRopeLimit = currRopeLength;
        }
        else if (isExtending && currRopeLength < maxRopeLimit)
        {
            target.distance += (Time.deltaTime * ropeSpeed);
            currRopeLength = target.distance;
            currMaxRopeLimit = currRopeLength;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // SET ROPE ============================================================
    void SetRope()
    {
        playerLinePos = new Vector2(transform.position.x, transform.position.y - 0.235f);
        partnerLinePos = new Vector2(Info.partner.transform.position.x, Info.partner.transform.position.y - 0.235f);

        line.SetPosition(0, playerLinePos);
        line.SetPosition(1, partnerLinePos);

        target.connectedAnchor = transform.position;
    }


    public void EnableRope(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !Info.isDead)
        {
            if (currDistFromPartner <= (maxTetherDist + 1) && !target.enabled)
            {
                Tethered(true);
            }
            else
            {
                Tethered(false);

                if(Info.latch.isLatched)
                {
                    Info.latch.ReleaseObject();
                }
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // PULL ================================================================
    void Pull()
    {
        target.frequency = ropeSpeed;

        if ((isExtending == false) && (isReeling == false))
        {
            if (currDistFromPartner < minTetherDist)
            {
                target.distance = minTetherDist;
            }
            else if (currDistFromPartner >= currMaxRopeLimit)
            {
                target.distance = currMaxRopeLimit;
            }
            else
            {
                target.distance = currDistFromPartner;
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // REEL IN =============================================================
    public void ReelIn(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !Info.isDead)
        {
            isReeling = true;
            isExtending = false;
        }
        else
        {
            isReeling = false;
        }

        // Logic for topsolid and partner
        if (topSolidMap)
        {
            topSolidMap.GetComponent<PlatformEffector2D>().colliderMask |= (1 << Info.partner.layer);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // EXTEND ==============================================================
    public void Extend(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isExtending = true;
            isReeling = false;
        }
        else
        {
            isExtending = false;
        }

        // Logic for topsolid and partner
        if (topSolidMap)
        {
            topSolidMap.GetComponent<PlatformEffector2D>().colliderMask &= ~(1 << Info.partner.layer);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TETHERED ============================================================
    void Tethered(bool tethered)
    {
        isTethered = tethered;
        target.enabled = tethered;
        line.enabled = tethered;
    }
}
