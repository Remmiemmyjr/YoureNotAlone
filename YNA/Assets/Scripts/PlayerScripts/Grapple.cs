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
    SpringJoint2D joint;

    Vector2 playerLinePos;
    Vector2 partnerLinePos;

    public float maxTetherDist = 1.5f;
    public float ropeSpeed = 3f;

    [HideInInspector]
    public float minRopeLimit = 0.25f;
    [HideInInspector]
    public float maxRopeLimit = 7f;
    
    float currMaxRopeLimit;
    float currDistFromPartner;
    float currRopeLength;

    [HideInInspector]
    public bool isTethered;

    bool isExtending;
    bool isReeling;

    public bool startTethered = true;

    public bool isMenu = false;
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
        joint = Info.partner?.GetComponent<SpringJoint2D>();

        if (joint != null && startTethered)
        {
            Tethered(true);
        }
        else
        {
            Tethered(false);
        }

        isExtending = false;
        isReeling = false;

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
            joint.distance -= (Time.deltaTime * ropeSpeed);
            currRopeLength = joint.distance;
            currMaxRopeLimit = currRopeLength;
        }
        else if (isExtending && currRopeLength < maxRopeLimit)
        {
            joint.distance += (Time.deltaTime * ropeSpeed);
            currRopeLength = joint.distance;
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

        joint.connectedAnchor = transform.position;
    }


    public void EnableRope(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !Info.isDead)
        {
            if (currDistFromPartner <= (maxTetherDist + 1) && !joint.enabled)
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
        if(!isMenu && GetComponent<PlayerController>().IsGrounded())
        {
            joint.frequency = ropeSpeed;
        }
        else
        {
            joint.frequency = ropeSpeed - 1.25f;
        }

        if ((isExtending == false) && (isReeling == false))
        {
            if (currDistFromPartner < minRopeLimit)
            {
                joint.distance = minRopeLimit;
            }
            // This is where tension/sqrting needs to happen
            else if (currDistFromPartner >= currMaxRopeLimit)
            {
                joint.distance = currMaxRopeLimit;
            }
            else
            {
                // if the curr distance between the characters is within the limit, set the joint's
                // distance to match their distance, so the player can walk through the partner
                joint.distance = currDistFromPartner;
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
        if (topSolidMap && Info.partner)
        {
            topSolidMap.GetComponent<PlatformEffector2D>().colliderMask |= (1 << Info.partner.layer);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // EXTEND ==============================================================
    public void Extend(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !Info.isDead)
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
        if(joint)
            joint.enabled = tethered;
        line.enabled = tethered;
    }
}
