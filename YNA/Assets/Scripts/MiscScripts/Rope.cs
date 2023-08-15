using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Rope;

public class Rope : MonoBehaviour
{
    // Struct representing one segment of the rope
    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        // Constructor for the segment
        public RopeSegment(Vector2 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }

    // Number of segments = Total distance / Segment size
    float distance;

    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    [HideInInspector]
    public float segmentLength = 0.25f;
    [HideInInspector]
    public int currRopeSize = 0;
    public int maxRopeSize = 28;

    [SerializeField]
    private float lineWidth = 0.1f;

    [SerializeField]
    float gravityScale = 5.0f;

    [SerializeField]
    private Transform playerPos;

    [SerializeField]
    private Transform partnerPos;

    // Start is called before the first frame update
    void Awake()
    {
        // Get the line renderer
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = transform.position; // Start at player position

        // Calculate the number of segments
        distance = Vector2.Distance(playerPos.position, partnerPos.position);
        currRopeSize = (int)(distance / segmentLength);

        // Create and add segments to list
        for (int i = 0; i < maxRopeSize; ++i)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= segmentLength; // Avoid overlap
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Runs at a fixed rate per frame
    private void FixedUpdate()
    {
        Simulate();
        DrawRope();
    }

    private void Simulate()
    {
        Vector2 forceGravity = new Vector2(0.0f, -gravityScale);

        for (int i = 0; i < currRopeSize; ++i) 
        {
            RopeSegment currSegment = ropeSegments[i];
            Vector2 velocity = currSegment.posNow - currSegment.posOld;
            currSegment.posOld = currSegment.posNow;
            currSegment.posNow += velocity;
            currSegment.posNow += forceGravity * Time.fixedDeltaTime;
            ropeSegments[i] = currSegment;
            ApplyConstraint();
        }

        //for (int i = 0; i < 50; ++i)
            //ApplyConstraint();
    }

    private void ApplyConstraint()
    {
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = playerPos.position;
        ropeSegments[0] = firstSegment;

        RopeSegment endSegment = ropeSegments[currRopeSize -1];
        endSegment.posNow = partnerPos.position;
        ropeSegments[currRopeSize -1] = endSegment;

        for (int i = 0; i < currRopeSize - 1; ++i)
        {
            RopeSegment currSegment = ropeSegments[i];
            RopeSegment nextSegment = ropeSegments[i + 1];

            float dist = (currSegment.posNow - nextSegment.posNow).magnitude;
            float error = Mathf.Abs(dist - segmentLength);
            Vector2 changeDir = Vector2.zero;

            if (dist > segmentLength)
                changeDir = (currSegment.posNow - nextSegment.posNow).normalized;
           
            else if (dist < segmentLength)
                changeDir = (nextSegment.posNow - currSegment.posNow).normalized;

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                currSegment.posNow -= changeAmount * 0.5f;
                ropeSegments[i] = currSegment;
                nextSegment.posNow += changeAmount * 0.5f;
                ropeSegments[i + 1] = nextSegment;
            }
            else
            {
                nextSegment.posNow += changeAmount;
                ropeSegments[i + 1] = nextSegment;
            }
        }
    }

    private void DrawRope()
    {
        float width = lineWidth;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        Vector3[] ropePositions = new Vector3[maxRopeSize];
        
        for (int i = 0; i < currRopeSize; ++i)
            ropePositions[i] = ropeSegments[i].posNow;

        lineRenderer.positionCount = currRopeSize;
        lineRenderer.SetPositions(ropePositions);
    }
}
