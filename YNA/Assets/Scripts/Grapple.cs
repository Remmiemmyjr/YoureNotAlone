using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grapple : MonoBehaviour
{
    GameObject partner;
    LineRenderer line;
    SpringJoint2D target;

    GameObject topSolidMap;

    //Vector3 newZPos;
    Vector2 playerLinePos;
    Vector2 partnerLinePos;

    public float maxLimit = 0.95f;
    public float minLimit = 0.75f;
    public float breakingPoint = 7f;
    public float pullSpeed = 1f;
    public float reelSpeed = 1.75f;
    public float airReelSpeed = 1f;

    float currDist;
    float currLength;

    [HideInInspector]
    public bool isTethered;

    bool isExtending;


    void Start()
    {
        partner = GameObject.FindGameObjectWithTag("Partner");
        topSolidMap = GameObject.FindGameObjectWithTag("TopSolidMap");

        line = GetComponent<LineRenderer>();
        target = partner?.GetComponent<SpringJoint2D>();
        if (target != null)
        {
            target.enabled = false;
        }
        line.enabled = false;
        isExtending = false;
        currLength = maxLimit;
    }



    void Update()
    {
        if(partner == null)
        {
            return;
        }
        EnableRope();

        currDist = (transform.position - partner.transform.position).magnitude;

        if (currDist > breakingPoint)
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


        if (Input.GetKey(KeyCode.LeftShift))
        {
            Extend();
            isExtending = true;
        }
        //else
        //{
        //    isExtending = false;
        //}
    }



    void EnableRope()
    {
        playerLinePos = new Vector2(transform.position.x, transform.position.y - 0.235f);
        partnerLinePos = new Vector2(partner.transform.position.x, partner.transform.position.y - 0.235f);

        line.SetPosition(0, playerLinePos);
        line.SetPosition(1, partnerLinePos);

        target.connectedAnchor = transform.position;

        if (Input.GetKeyDown(KeyCode.X))
        {
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



    void Pull()
    {
        target.frequency = pullSpeed;

        if (isExtending == false)
        {
            if (currDist < minLimit)
            {
                target.distance = minLimit;
            }                 //max
            else if (currDist < currLength)
            {
                target.distance = currDist;
            }
            else
            {                   //max
                target.distance = currLength;
            }
        }
    }



    void ReelIn()
    {
        isExtending = false;
        target.distance = 0;
        currLength = maxLimit;
        maxLimit = 0.95f;
        if(!gameObject.GetComponent<PlayerController>().onGround)
        {
            target.frequency = airReelSpeed;
        }
        else
        {
            target.frequency = reelSpeed;
        }

        // Logic for topsolid and partner
        topSolidMap.GetComponent<PlatformEffector2D>().colliderMask |= (1 << partner.layer);
    }



    void Extend()
    {
        Debug.Log("extending");
        target.distance += 0.02f;

        // Logic for topsolid and partner
        topSolidMap.GetComponent<PlatformEffector2D>().colliderMask &= ~(1 << partner.layer);
    }
}
