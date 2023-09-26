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
    public float tensionScalar = 100f;
    private float tension;

    [HideInInspector]
    public float minRopeLimit;
    float ogMinLimit;
    public float maxRopeLimit = 5f;

    float currMaxRopeLimit;
    float currRopeLength;

    [HideInInspector]
    public bool isTethered;

    bool isExtending;
    bool isReeling;

    public bool isMenu = false;

    [SerializeField]
    public bool startTetheredTogether = true;

    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    private void Awake()
    {
        minRopeLimit = 0.25f;
        //maxRopeLimit = 5f;
        ogMinLimit = minRopeLimit;

        topSolidMap = GameObject.FindGameObjectsWithTag("TopSolidMap");
        line = GetComponent<LineRenderer>();
    }

    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        if(Retether.relink == true)
        {
            startTetheredTogether = true;
        }

        rope = GetComponent<Rope>();
        joint = Info.partner?.GetComponent<SpringJoint2D>();
        playerController = Info.player?.GetComponent<PlayerController>();


        // Apply tension
        tension = tensionScalar * maxRopeLimit * 100;

        if (joint && startTetheredTogether)
        {
            Tethered(true);
        }
        else if (joint && !startTetheredTogether)
        {
            Tethered(false);
        }

        isExtending = false;
        isReeling = false;

        currRopeLength = minRopeLimit;
        currMaxRopeLimit = currRopeLength;
        SetRope();

        if (joint)
            joint.distance = currMaxRopeLimit;
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        if (Info.partner == null || Info.isDead)
        {
            return;
        }

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
        if (rope.currRopeListSize <= rope.maxRopeListSize)
        {
            rope.currRopeListSize = (int)(currMaxRopeLimit / rope.segmentLength);

            if (rope.currRopeListSize > rope.maxRopeListSize)
                rope.currRopeListSize = rope.maxRopeListSize;

            else if (rope.currRopeListSize < rope.minRopeListSize)
                rope.currRopeListSize = rope.minRopeListSize;
        }

        if (joint)
            joint.connectedAnchor = transform.position;
    }


    ////////////////////////////////////////////////////////////////////////
    // ENABLE ROPE =========================================================
    public void EnableRope(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !Info.isDead && Info.partner)
        {
            if (currDistFromPartner.magnitude <= (maxTetherDist + 1) && !joint.enabled)
            {
                Tethered(true);


                currRopeLength = maxTetherDist;
                currMaxRopeLimit = currRopeLength;
                joint.distance = currMaxRopeLimit;
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
        RaycastHit2D hitPlayer = Physics2D.Linecast(Info.player.transform.position, Info.partner.transform.position, 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D hitPartner = Physics2D.Linecast(Info.partner.transform.position, Info.player.transform.position, 1 << LayerMask.NameToLayer("Ground"));

        // Sent out a ray between the player and the partner in both directions and set the minimum rope distance to the distance between the raycasts
        if (hitPlayer && hitPartner)
        {
            float distance = Vector3.Distance(hitPlayer.point, hitPartner.point);
            minRopeLimit = distance + ogMinLimit;
        }
        else
        {
            minRopeLimit = ogMinLimit;
        }

        if (!isMenu && playerController.IsGrounded())
        {
            joint.frequency = ropeSpeed;
        }
        else
        {
            joint.frequency = ropeSpeed - 1.25f;
        }

        if (!isMenu && (isExtending == false) && (isReeling == false))
        {
            // This is where tension/sqrting needs to happen
            if (currDistFromPartner.magnitude >= currMaxRopeLimit)
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

            // Check the new joint distance
            if (joint.distance < minRopeLimit)
            {
                joint.distance = minRopeLimit;
                //currMaxRopeLimit = minRopeLimit;
                //rope.ResetRope();

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
    public void Tethered(bool tethered)
    {
        isTethered = tethered;
        if (joint)
            joint.enabled = tethered;
        line.enabled = tethered;
    }
}