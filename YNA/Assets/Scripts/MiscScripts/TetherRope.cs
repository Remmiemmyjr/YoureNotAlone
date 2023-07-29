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

    List<GameObject> Rope = new List<GameObject>();

    public float segmentDist = 0.25f;
    public int maxDist = 10;
    public int minDist = 3;

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

        if (Input.GetKeyDown(KeyCode.B))
        {
            AddRopeSegment();
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
        Rope.Add(newNode);

        lastNode.GetComponent<HingeJoint2D>().connectedBody = newNode.GetComponent<Rigidbody2D>();
        lastNode = newNode;
    }

    void DestroyRope()
    {
        for(int i = Rope.Count - 1; i >= 0; i--)
        {
            GameObject currNode = Rope[i];
            currNode.GetComponent<HingeJoint2D>().enabled = false;
            Rope.Remove(currNode);
            Destroy(currNode);
        }
        Rope.Clear();
        lastNode = transform.gameObject;
    }

    void RemoveRopeSegment()
    {
        if (Rope.Count > minDist)
        {
            Rope.Remove(lastNode);
            Destroy(lastNode);

            lastNode = Rope.Last();
            lastNode.GetComponent<HingeJoint2D>().connectedBody = targetPartner.GetComponent<Rigidbody2D>();

            targetPartner.transform.position = Vector2.MoveTowards(targetPartner.transform.position, lastNode.transform.position, 0.25f);

            lastNode.tag = "Last";
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
                    //yield return new WaitForSeconds(1.0f);
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
