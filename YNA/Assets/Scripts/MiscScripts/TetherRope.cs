using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TetherRope : MonoBehaviour
{
    public GameObject ropeSegment;
    public GameObject lastNode;
    public GameObject targetPartner;

    public float segmentDistance;
    bool isTethered;

    // Start is called before the first frame update
    void Awake()
    {
        lastNode = transform.gameObject;
        isTethered = false;
        GetComponent<HingeJoint2D>().enabled = false;
        targetPartner.GetComponent<HingeJoint2D>().enabled = false;
    }

    void Update()
    {

    }

    public void Tether(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isTethered = !isTethered;
            StartCoroutine(ManageRope(isTethered));
        }
    }

    void AddRopeSegment()
    {
        Vector2 segPos = targetPartner.transform.position - lastNode.transform.position;
        segPos.Normalize();

        segPos *= segmentDistance;
        segPos += (Vector2)(lastNode.transform.position);

        GameObject newNode = Instantiate(ropeSegment, segPos, Quaternion.identity);
        newNode.transform.parent = transform;

        lastNode.GetComponent<HingeJoint2D>().connectedBody = newNode.GetComponent<Rigidbody2D>();

        lastNode = newNode;
    }

    void DestroyRope()
    {

    }

    void RemoveRopeSegment()
    {

    }

    IEnumerator ManageRope(bool tethered)
    {
        switch(tethered)
        {
            // If player should be tethered: --------------------------------------------
            case true:
                GetComponent<HingeJoint2D>().enabled = true;
                targetPartner.GetComponent<HingeJoint2D>().enabled = true;
                //lastNode.GetComponent<HingeJoint2D>().connectedBody = targetPartner.GetComponent<Rigidbody2D>();
                targetPartner.GetComponent<HingeJoint2D>().connectedAnchor = (Vector2)lastNode.transform.position;

                while (Vector2.Distance(targetPartner.transform.position, lastNode.transform.position) > segmentDistance)
                {
                    AddRopeSegment();
                }
                break;

            // If player should NOT be tethered: ----------------------------------------
            case false:
                GetComponent<HingeJoint2D>().enabled = false;
                targetPartner.GetComponent<HingeJoint2D>().enabled = false;

                DestroyRope();
                break;
        }
        yield return null;
    }

}
