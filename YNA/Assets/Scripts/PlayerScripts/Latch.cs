using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Latch : MonoBehaviour
{
    Grapple playerGrapple;
    Rigidbody2D objRB;
    HingeJoint2D joint;
    float ogMass;

    private void Awake()
    {
        playerGrapple = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Grapple>();

    }

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

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Grabbable" && playerGrapple.isLatching)
    //    {
    //        playerGrapple.canLatch = false;
    //    }
    //}

    public void GrabObject()
    {
        joint.connectedBody = GetComponentInParent<Rigidbody2D>();
        objRB.mass = 1.0f;
        joint.enabled = true;
    }

    public void ReleaseObject()
    {
        joint.enabled = false;
        playerGrapple.canLatch = false;
        joint.connectedBody = null;
        objRB.mass = ogMass;
        objRB = null;
    }
}
