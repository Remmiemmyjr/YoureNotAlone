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

public class Latch : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    Grapple playerGrapple;
    Rigidbody2D objRB;
    HingeJoint2D joint;
    float ogMass;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    private void Awake()
    {
        playerGrapple = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Grapple>();
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grabbable")
        {
            playerGrapple.canLatch = true;
            joint = collision.gameObject.GetComponent<HingeJoint2D>();
            objRB = collision.GetComponent<Rigidbody2D>();
            ogMass = objRB.mass;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // GRAB OBJECT =========================================================
    public void GrabObject()
    {
        joint.connectedBody = GetComponentInParent<Rigidbody2D>();
        objRB.mass = 1.0f;
        joint.enabled = true;
    }


    ////////////////////////////////////////////////////////////////////////
    // RELEASE OBJECT ======================================================
    public void ReleaseObject()
    {
        joint.enabled = false;
        playerGrapple.canLatch = false;
        joint.connectedBody = null;
        objRB.mass = ogMass;
        objRB = null;
    }
}
