using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempNewJoint : MonoBehaviour
{
    DistanceJoint2D joint;
    LineRenderer line;

    Vector2 playerLinePos, partnerLinePos;

    public float maxTetherDist = 1.5f;
    public float minTetherDist = 0.35f;
    public float ropeSpeed = 3f;

    [HideInInspector]
    public float minRopeLimit = 0.25f;
    [HideInInspector]
    public float maxRopeLimit = 10f;

    float currMaxRopeLimit;
    float currDistFromPartner;

    [HideInInspector]
    public bool isTethered;

    bool isExtending;
    bool isReeling;


    private void Awake()
    {
        joint = GetComponent<DistanceJoint2D>();
        line = GetComponent<LineRenderer>();
    }

    
    void Start()
    {
        joint.connectedBody = Info.partner.GetComponent<Rigidbody2D>();
        joint.enabled = true;

        isReeling = false;
        isExtending = false;
        
        currMaxRopeLimit = maxTetherDist;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateRope();
        RopePhysics();

        currDistFromPartner = (transform.position - Info.partner.transform.position).magnitude;
    }


    void UpdateRope()
    {
        playerLinePos = new Vector2(transform.position.x, transform.position.y - 0.235f);
        partnerLinePos = new Vector2(Info.partner.transform.position.x, Info.partner.transform.position.y - 0.235f);

        line.SetPosition(0, playerLinePos);
        line.SetPosition(1, partnerLinePos);
    }


    void RopePhysics()
    {
        if ((isExtending == false) && (isReeling == false))
        {
            if (currDistFromPartner < minTetherDist)
            {
                joint.distance = minTetherDist;
            }
            else if (currDistFromPartner >= currMaxRopeLimit)
            {
                joint.distance = currMaxRopeLimit;
            }
            else
            {
                joint.distance = currDistFromPartner;
            }
        }

        if (isReeling && currDistFromPartner > minRopeLimit)
        {
            joint.distance -= (Time.deltaTime * ropeSpeed);
            currMaxRopeLimit = joint.distance;
        }
        else if (isExtending && currDistFromPartner < maxRopeLimit)
        {
            joint.distance += (Time.deltaTime * ropeSpeed);
            currMaxRopeLimit = joint.distance;
        }
    }


    public void EnableRope(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !Info.isDead)
        {
            if (currDistFromPartner <= (maxTetherDist + 1) && !joint.enabled)
            {
                Tethered(true);
            }
            else
            {
                Tethered(false);

                if (Info.latch.isLatched)
                {
                    Info.latch.ReleaseObject();
                }
            }
        }
    }

    public void ReelIn(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !Info.isDead)
        {
            isReeling = true;
            isExtending = false;
        }
        else
        {
            isReeling = false;
        }
    }


    public void Extend(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isExtending = true;
            isReeling = false;
        }
        else
        {
            isExtending = false;
        }
    }


    void Tethered(bool tethered)
    {
        isTethered = tethered;
        joint.enabled = tethered;
        line.enabled = tethered;
    }
}
