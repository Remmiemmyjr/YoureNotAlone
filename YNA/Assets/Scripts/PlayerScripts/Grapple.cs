//*************************************************
// Project: We're Tethered Together
// File: Grapple.cs
// Author/s: Emmy Berg
//           Corbyn Lamar
//           Mike Doeren
//
// Desc: Tethering controls & actions for player
//
// Last Edit: 8/14/2023
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
    GameObject[] topSolidMap;
    LineRenderer line;
    SpringJoint2D joint;
    PlayerController playerController;
    Rope rope;

    Vector2 currDistFromPartner;

    public float maxTetherDist = 1.5f;
    public float ropeSpeed = 3f;
    public float tensionScalar = 10f;
    private float tension;

    [HideInInspector]
    public float minRopeLimit = 0.25f;
    [HideInInspector]
    public float maxRopeLimit = 7f;
    
    float currMaxRopeLimit;
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
        topSolidMap = GameObject.FindGameObjectsWithTag("TopSolidMap");
        line = GetComponent<LineRenderer>();
    }

    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        rope = GetComponent<Rope>();
        joint = Info.partner?.GetComponent<SpringJoint2D>();
        playerController = Info.player?.GetComponent<PlayerController>();

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

        currRopeLength = minRopeLimit;
        currMaxRopeLimit = currRopeLength;
        joint.distance = currMaxRopeLimit;
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        if (Info.partner == null)
        {
            return;
        }

        tension = tensionScalar * (1/maxRopeLimit) * 1000;

        currDistFromPartner = (Info.partner.transform.position - transform.position);
        SetRope();

        if (isTethered)
        {
            Pull();
        }
        else
        {
            if (!isMenu)
                playerController.currSpeed = playerController.speed;
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
        if (rope.currRopeSize <= rope.maxRopeSize && currDistFromPartner.magnitude > 1)
        {
            rope.currRopeSize = (int)(currDistFromPartner.magnitude / rope.segmentLength);

            if (rope.currRopeSize > rope.maxRopeSize)
                rope.currRopeSize = rope.maxRopeSize;           
        }

        joint.connectedAnchor = transform.position;
    }


    ////////////////////////////////////////////////////////////////////////
    // ENABLE ROPE =========================================================
    public void EnableRope(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !Info.isDead)
        {
            if (currDistFromPartner.magnitude <= (maxTetherDist + 1) && !joint.enabled)
            {
                Tethered(true);

                currRopeLength = maxTetherDist;
                currMaxRopeLimit = currRopeLength;
            }
            else
            {
                Tethered(false);

                if (Info.latch.isLatched)
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
        if(!isMenu && playerController.IsGrounded())
        {
            joint.frequency = ropeSpeed;
        }
        else
        {
            joint.frequency = ropeSpeed - 1.25f;
        }

        if (!isMenu && (isExtending == false) && (isReeling == false))
        {
            if (currDistFromPartner.magnitude < minRopeLimit)
            {
                joint.distance = minRopeLimit;
                currMaxRopeLimit = minRopeLimit;
            }
            // This is where tension/sqrting needs to happen
            else if (currDistFromPartner.magnitude >= currMaxRopeLimit)
            {
                joint.distance = currMaxRopeLimit;

                // Check if the player is trying to move away from the partner by evaluating the signs of their direction
                if (Mathf.Sign(currDistFromPartner.x) != Mathf.Sign(playerController.dir.x) && currDistFromPartner.magnitude > currMaxRopeLimit + 1)
                {
                    // If players moving away, apply a force of tension using the inverse exponential formula
                    playerController.currSpeed = playerController.currSpeed / (1 + (Mathf.Pow(currDistFromPartner.magnitude, 2) / tension));
                }
                // Otherwise, they are trying to move towards the partner and should experience no tension
                else
                {
                    playerController.currSpeed = playerController.speed;
                }
            }
            else
            {
                joint.distance = currDistFromPartner.magnitude;
                playerController.currSpeed = playerController.speed;
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
        if (topSolidMap.Length > 0 && Info.partner)
        {
            foreach (GameObject topSolid in topSolidMap)
            {
                topSolid.GetComponent<PlatformEffector2D>().colliderMask |= (1 << Info.partner.layer);
            }
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
        if (topSolidMap.Length > 0 && Info.partner)
        {
            foreach (GameObject topSolid in topSolidMap)
            {
                topSolid.GetComponent<PlatformEffector2D>().colliderMask &= ~(1 << Info.partner.layer);
            }
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
