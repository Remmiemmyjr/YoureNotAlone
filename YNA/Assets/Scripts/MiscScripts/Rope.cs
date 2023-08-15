using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Rope;

public class Rope : MonoBehaviour
{
    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

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
    public int numSegments = 0; // 35
    public int maxSegments = 28;

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
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = transform.position;

        distance = Vector2.Distance(playerPos.position, partnerPos.position);
        numSegments = (int)(distance / segmentLength);

        for (int i = 0; i < maxSegments; ++i)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= segmentLength; // Avoid overlap
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawRope();
    }

    // Runs at a fixed rate per frame
    private void FixedUpdate()
    {
        Simulate();
    }

    private void Simulate()
    {
        Vector2 forceGravity = new Vector2(0.0f, -gravityScale);

        for (int i = 0; i < numSegments; ++i) 
        {
            RopeSegment currSegment = ropeSegments[i];
            Vector2 velocity = currSegment.posNow - currSegment.posOld;
            currSegment.posOld = currSegment.posNow;
            currSegment.posNow += velocity;
            currSegment.posNow += forceGravity * Time.deltaTime;
            ropeSegments[i] = currSegment;
        }
        for (int i = 0; i < 50; ++i)
            ApplyConstraint();
    }

    private void ApplyConstraint()
    {
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = playerPos.position;
        ropeSegments[0] = firstSegment;

        RopeSegment endSegment = ropeSegments[numSegments -1];
        endSegment.posNow = partnerPos.position;
        ropeSegments[numSegments -1] = endSegment;

        for (int i = 0; i < numSegments - 1; ++i)
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

        Vector3[] ropePositions = new Vector3[maxSegments];
        
        for (int i = 0; i < numSegments; ++i)
            ropePositions[i] = ropeSegments[i].posNow;

        lineRenderer.positionCount = numSegments;
        lineRenderer.SetPositions(ropePositions);
    }
}
