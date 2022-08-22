using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    GameObject partner;
    LineRenderer line;
    SpringJoint2D target;

    Vector3 newZPos;

    public float maxLimit = 1.5f;
    public float minLimit = 1f;
    public float breakingPoint = 4f;
    public float pullSpeed = 1f;
    public float reelSpeed = 2f;
    public float airReelSpeed = 1.15f;

    float currDist;

    [HideInInspector]
    public bool isTethered;


    void Start()
    {
        partner = GameObject.FindGameObjectWithTag("Partner");

        line = GetComponent<LineRenderer>();
        target = partner.GetComponent<SpringJoint2D>();

        target.enabled = false;
        line.enabled = false;
    }



    void Update()
    {
        EnableRope();

        currDist = (transform.position - partner.transform.position).magnitude;

        if(currDist > breakingPoint)
        {
            line.enabled = false;
            target.enabled = false;
        }


        if (line.enabled)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                ReelIn();
            }
            else
            {
                Pull();
            }
        }

        newZPos = new Vector3(transform.position.x, transform.position.y, 0);
    }



    void EnableRope()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, partner.transform.position);

        target.connectedAnchor = transform.position;

        if (Input.GetKeyDown(KeyCode.X))
        {
            line.transform.position = newZPos;
            

            if (currDist <= (maxLimit + 1) && !target.enabled)
            {
                target.enabled = true;
                line.enabled = true;
                isTethered = true;
            } 
            else
            {
                target.enabled = false;
                line.enabled = false;
                isTethered = false;
            }
        }
    }

    //if (Input.GetKeyDown(KeyCode.X))
    //{
    //    if (currDist <= (maxLimit + 1) && target.enabled)
    //    {
    //        target.enabled = !target.enabled;
    //        line.enabled = !line.enabled;
    //    } 
    //}

    void Pull()
    {
        target.frequency = pullSpeed;

        if (currDist < minLimit)
        {
            target.distance = minLimit;
        }
        else if (currDist < maxLimit)
        {
            target.distance = currDist;
        }
        else
        {
            target.distance = maxLimit;
        }
    }



    void ReelIn()
    {
        target.distance = 0;
        if(!gameObject.GetComponent<PlayerController>().onGround)
        {
            target.frequency = airReelSpeed;
        }
        else
        {
            target.frequency = reelSpeed;
        }
    }
}
