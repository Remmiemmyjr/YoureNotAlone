//*************************************************
// Project: We're Tethered Together
// File: Latch.cs
// Author/s: Emmy Berg
//
// Desc:  Manages the mechanic that allows boxes
//        to be latched to partner when in range
//
// Notes:
// - 
//
// Last Edit: 7/16/2023
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
    Rigidbody2D objRB;
    HingeJoint2D joint;
    float ogMass;
    float ogMinLimit;
    public bool isLatched;
    bool canLatch;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        isLatched = false;
        
    }

    void Start()
    {
        ogMinLimit = Info.grapple.minRopeLimit;
    }

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
        if (collision.gameObject.tag == "Grabbable")
        {
            canLatch = true;

            joint = collision.gameObject.GetComponent<HingeJoint2D>();
            objRB = collision.GetComponent<Rigidbody2D>();

            ogMass = objRB.mass;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grabbable" && !isLatched)
        {
            canLatch = false;
            joint = null;
            objRB = null;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // GRAB OBJECT =========================================================
    public void GrabObject()
    {
        joint.connectedBody = GetComponentInParent<Rigidbody2D>();
        joint.enabled = true;

        objRB.mass = 0.5f;
        objRB.freezeRotation = true;

        Info.grapple.minRopeLimit = 1.75f;
    }


    ////////////////////////////////////////////////////////////////////////
    // RELEASE OBJECT ======================================================
    public void ReleaseObject()
    {
        canLatch = false;

        joint.enabled = false;
        joint.connectedBody = null;
        joint = null;

        objRB.mass = ogMass;
        objRB.freezeRotation = false;
        objRB = null;

        Info.grapple.minRopeLimit = ogMinLimit;
    }
}
