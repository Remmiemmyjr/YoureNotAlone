using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    GameObject partner;
    LineRenderer line;
    SpringJoint2D target;

    public float maxLimit = 5f;
    public float minLimit = 0.75f;


    // Start is called before the first frame update
    void Start()
    {
        partner = GameObject.FindGameObjectWithTag("Partner");

        line = GetComponent<LineRenderer>();
        target = partner.GetComponent<SpringJoint2D>();


        target.enabled = false;
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        EnableRope();

        float currDist = (transform.position - partner.transform.position).magnitude;

        if(currDist < minLimit)
        {
            target.distance = minLimit;
        }
        else if(currDist < maxLimit)
        {
            target.distance = currDist;
        }
        else
        {
            target.distance = maxLimit;
        }
    }

    void EnableRope()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, partner.transform.position);

        target.connectedAnchor = transform.position;

        if (Input.GetKeyDown(KeyCode.X))
        {
            target.enabled = !target.enabled;
            line.enabled = !line.enabled;
        }
    }
}
