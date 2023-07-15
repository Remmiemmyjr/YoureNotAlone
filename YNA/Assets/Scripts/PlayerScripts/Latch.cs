using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Latch : MonoBehaviour
{
    Grapple playerGrapple;
    GameObject obj;
    HingeJoint2D joint;

    private void Awake()
    {
        playerGrapple = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Grapple>();
        joint = GetComponent<HingeJoint2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grabbable")
        {
            playerGrapple.canLatch = true;
            obj = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grabbable")
        {
            playerGrapple.canLatch = false;
            obj = null;
        }
    }

    public void GrabObject()
    {
        //obj.transform.parent = transform;
        joint.connectedBody = obj.GetComponent<Rigidbody2D>();
        //joint.anchor = obj.transform.position;
    }

    public void ReleaseObject()
    {
        //obj.transform.parent = null;
        joint.connectedBody = null;
        //joint.anchor = transform.position;
    }
}
