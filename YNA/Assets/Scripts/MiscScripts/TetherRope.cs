using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TetherRope : MonoBehaviour
{
    public GameObject ropeSegment;
    public GameObject lastNode;
    public GameObject targetPartner;

    Queue<GameObject> Rope = new Queue<GameObject>();

    public float segmentDist = 0.25f;
    public float maxDist = 2.0f;
    public float minDist = 0.3f;

    float currDist;
    bool isTethered;

    // Start is called before the first frame update
    void Awake()
    {
        lastNode = transform.gameObject;
        isTethered = false;
        GetComponent<HingeJoint2D>().enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            RemoveRopeSegment();
        }
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

        segPos *= segmentDist;
        segPos += (Vector2)(lastNode.transform.position);

        GameObject newNode = Instantiate(ropeSegment, segPos, Quaternion.identity);
        //Rope.PushFront(newNode);
        Rope.Enqueue(newNode);


        lastNode.GetComponent<HingeJoint2D>().connectedBody = newNode.GetComponent<Rigidbody2D>();

        lastNode = newNode;
    }

    void DestroyRope()
    {
        
    }

    void RemoveRopeSegment()
    {
        if (Rope.Count > 0)
        {
            GameObject prevNode = Rope.ElementAt(Rope.Count - 2);
            prevNode.GetComponent<HingeJoint2D>().connectedBody = targetPartner.GetComponent<Rigidbody2D>();
            
            Destroy(Rope.Last());
            Rope.Dequeue();

            lastNode = prevNode;
        }
    }

    IEnumerator ManageRope(bool tethered)
    {
        switch(tethered)
        {
            // If player should be tethered: --------------------------------------------
            case true:
                GetComponent<HingeJoint2D>().enabled = true;

                while (Vector2.Distance(targetPartner.transform.position, lastNode.transform.position) > segmentDist)
                {
                    AddRopeSegment();
                }
                
                lastNode.GetComponent<HingeJoint2D>().connectedBody = targetPartner.GetComponent<Rigidbody2D>();
                lastNode.tag = "Last";
                break;

            // If player should NOT be tethered: ----------------------------------------
            case false:
                GetComponent<HingeJoint2D>().enabled = false;

                DestroyRope();
                break;
        }
        yield return null;
    }

}
