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
    public bool isLatched;
    bool canLatch;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        DontDestroyOnLoad(this);
        isLatched = false;
    }

    void Update()
    {
    }


    public void LatchBox(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
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
        objRB.mass = 0.5f;
        joint.enabled = true;
    }


    ////////////////////////////////////////////////////////////////////////
    // RELEASE OBJECT ======================================================
    public void ReleaseObject()
    {
        canLatch = false;
        joint.enabled = false;
        joint.connectedBody = null;
        objRB.mass = ogMass;
        objRB = null;
    }
}
