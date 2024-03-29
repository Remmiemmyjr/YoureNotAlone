//*************************************************
// Project: We're Tethered Together
// File: Latch.cs
// Author/s: Emmy Berg
//           Corbyn LaMar
//
// Desc:  Manages the mechanic that allows boxes
//        to be latched to partner when in range
//
// Notes:
// - 
//
// Last Edit: 8/13/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Latch : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    GameObject obj;
    HingeJoint2D joint;
    float ogMass;
    float ogMinLimit;
    [HideInInspector]
    public bool isLatched;
    bool canLatch;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        isLatched = false;
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        ogMinLimit = Info.grapple.minRopeLimit;
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    private void Update()
    {
        if(!canLatch && !isLatched)
        {
            obj?.GetComponent<BoxStats>().SetNormalMat();
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // LATCH BOX ===========================================================
    public void LatchBox(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canLatch)
        {
            if (Info.grapple.isTethered)
            {
                if (!isLatched && canLatch)
                {
                    isLatched = true;
                    GrabObject();
                }
                else
                {
                    isLatched = false;
                    ReleaseObject();
                }
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grabbable" && !Info.isDead && !isLatched)
        {
            canLatch = true;

            joint = collision.gameObject.GetComponent<HingeJoint2D>();
            obj = collision.gameObject;

            ogMass = obj.GetComponent<Rigidbody2D>().mass;

            if (obj)
            {
                obj.GetComponent<BoxStats>().SetOutlineMat(false);
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER EXIT ========================================================
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grabbable" && !isLatched)
        {
            obj?.GetComponent<BoxStats>().SetNormalMat();

            canLatch = false;
            joint = null;
            obj = null;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // GRAB OBJECT =========================================================
    public void GrabObject()
    {
        joint.connectedBody = GetComponentInParent<Rigidbody2D>();
        joint.enabled = true;

        if (obj)
        {
            obj.GetComponent<BoxStats>().SetOutlineMat(true);
            obj.GetComponent<BoxCollider2D>().enabled = false;

            Vector3 partnerPos = Info.partner.transform.position;
            partnerPos.z = 1;
            obj.transform.position = partnerPos;

            obj.transform.SetParent(Info.partner.transform);

            obj.GetComponent<Rigidbody2D>().mass = 0.5f;
            obj.GetComponent<Rigidbody2D>().freezeRotation = true;
        }

        Info.grapple.minRopeLimit += 1f;
    }


    ////////////////////////////////////////////////////////////////////////
    // RELEASE OBJECT ======================================================
    public void ReleaseObject()
    {
        canLatch = false;
        isLatched = false;

        if (joint)
        {
            joint.enabled = false;
            joint.connectedBody = null;
        }
        joint = null;

        if (obj)
        {
            obj.GetComponent<Rigidbody2D>().mass = ogMass;
            obj.GetComponent<Rigidbody2D>().freezeRotation = false;
            
            obj.GetComponent<BoxStats>().SetNormalMat();
            obj.GetComponent<BoxCollider2D>().enabled = true;

            obj.transform.SetParent(null);

            //RaycastHit2D hitPlayer = Physics2D.Linecast(Info.player.transform.position, Info.partner.transform.position, 1 << LayerMask.NameToLayer("Ground"));
            //RaycastHit2D hitPartner = Physics2D.Linecast(Info.partner.transform.position, Info.player.transform.position, 1 << LayerMask.NameToLayer("Ground"));

            //// Sent out a ray between the player and the partner in both directions and set the minimum rope distance to the distance between the raycasts
            //if (hitPlayer && hitPartner)
            //{
            //    float distance = Vector3.Distance(hitPlayer.point, hitPartner.point);
            //    Info.grapple.minRopeLimit = distance + ogMinLimit;
            //}
            //else
            //{
            //    Info.grapple.minRopeLimit = ogMinLimit;
            //}
        }

        obj = null;
    }
}
