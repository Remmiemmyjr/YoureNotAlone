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
            obj = collision.gameObject;

            ogMass = obj.GetComponent<Rigidbody2D>().mass;

            obj.GetComponent<BoxStats>().SetOutlineMat(false);
        }
    }

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

        obj.GetComponent<Rigidbody2D>().mass = 0.5f;
        obj.GetComponent<Rigidbody2D>().freezeRotation = true;

        Info.grapple.minRopeLimit += 1.5f;

        // TODO - CHANGE SHADER MATERIAL TO SOMETHING UNIQUE WHILE GRABBED
        obj.GetComponent<BoxStats>().SetOutlineMat(true);
    }


    ////////////////////////////////////////////////////////////////////////
    // RELEASE OBJECT ======================================================
    public void ReleaseObject()
    {
        canLatch = false;

        joint.enabled = false;
        joint.connectedBody = null;
        joint = null;

        obj.GetComponent<Rigidbody2D>().mass = ogMass;
        obj.GetComponent<Rigidbody2D>().freezeRotation = false;

        Info.grapple.minRopeLimit = ogMinLimit;
        obj.GetComponent<BoxStats>().SetNormalMat();
        obj = null;
    }
}
