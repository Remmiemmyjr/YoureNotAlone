using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TetherRope : MonoBehaviour
{
    public GameObject segmentPrefab;
    public GameObject lastNode;
    public GameObject targetPartner;

    List<GameObject> RopeList = new List<GameObject>();

    public float segmentOffset = 0.25f;
    public int maxNodes = 10;
    public int minNodes = 3;

    bool isTethered;

    // Start is called before the first frame update
    void Awake()
    {
        lastNode = transform.gameObject;
        isTethered = false;
        GetComponent<HingeJoint2D>().enabled = false;
    }

    private void Start()
    {
        Tether();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    RemoveRopeSegment();
        //}

        if (Input.GetKeyDown(KeyCode.B))
        {
            Tether();
        }
    }

    public void Tether()
    {
        isTethered = !isTethered;
        StartCoroutine(ManageRope(isTethered));
        
    }

    void AddRopeSegment(GameObject obj)
    {
        Vector2 segPos = (transform.position - new Vector3(0, 5)) - lastNode.transform.position;
        segPos.Normalize();

        segPos *= segmentOffset;
        segPos += (Vector2)(lastNode.transform.position);

        GameObject newNode = Instantiate(obj, segPos, Quaternion.identity);
        RopeList.Add(newNode);

        lastNode.GetComponent<HingeJoint2D>().connectedBody = newNode.GetComponent<Rigidbody2D>();
        lastNode = newNode;
    }

    void DestroyRope()
    {
        for(int i = RopeList.Count - 1; i >= 0; i--)
        {
            GameObject currNode = RopeList[i];
            currNode.GetComponent<HingeJoint2D>().enabled = false;
            RopeList.Remove(currNode);
            Destroy(currNode);
        }
        RopeList.Clear();
        lastNode = transform.gameObject;
    }

    //void RemoveRopeSegment()
    //{
    //    if (RopeList.Count > minNodes)
    //    {
    //        RopeList.Remove(lastNode);
    //        Destroy(lastNode);

    //        lastNode = RopeList.Last();
    //        lastNode.GetComponent<HingeJoint2D>().connectedBody = targetPartner.GetComponent<Rigidbody2D>();

    //        targetPartner.transform.position = Vector2.MoveTowards(targetPartner.transform.position, lastNode.transform.position, 0.25f);

    //        lastNode.tag = "Last";
    //    }
    //}

    IEnumerator ManageRope(bool tethered)
    {
        switch(tethered)
        {
            // If player should be tethered: --------------------------------------------
            case true:
                GetComponent<HingeJoint2D>().enabled = true;

                while (Vector2.Distance(transform.position - new Vector3(0, 5), lastNode.transform.position) > segmentOffset)
                {
                    AddRopeSegment(segmentPrefab);
                    //yield return new WaitForSeconds(1.0f);
                }
                AddRopeSegment(targetPartner);
                
                //lastNode.GetComponent<HingeJoint2D>().connectedBody = targetPartner.GetComponent<Rigidbody2D>();
                //lastNode.tag = "Last";
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
