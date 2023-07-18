using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Rope;

public class Rope : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private float segmentLength = 0.25f;
    private int numSegments = 35;

    [SerializeField]
    private float lineWidth = 0.1f;

    [SerializeField]
    float gravityScale = 5.0f;
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
    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = transform.position;

        for (int i = 0; i < numSegments; ++i) 
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
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.deltaTime;
            ropeSegments[i] = firstSegment;
        }
        for (int i = 0; i < 50; ++i)
            ApplyConstraint();
    }

    private void ApplyConstraint()
    {
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = transform.position;
        ropeSegments[0] = firstSegment;

        for (int i = 0; i < numSegments - 1; ++i)
        {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - segmentLength);
            Vector2 changeDir = Vector2.zero;

            if (dist > segmentLength)
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
           
            else if (dist < segmentLength)
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope()
    {
        float width = lineWidth;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        Vector3[] ropePositions = new Vector3[numSegments];
        
        for (int i = 0; i < numSegments; ++i)
            ropePositions[i] = ropeSegments[i].posNow;

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }
}
