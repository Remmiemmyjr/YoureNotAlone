//*************************************************
// Project: We're Tethered Together
// File: Rope.cs
// Author/s: Emmy Berg
//           Mike Doeren
//           Corbyn LaMar
//
// Desc: Manage player actions
//
// Notes:
// -
//
// Last Edit: 8/24/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
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
    public float segmentLength;
    [HideInInspector]
    public int currRopeListSize = 0;
    [HideInInspector]
    public int maxRopeListSize = 70;
    [HideInInspector]
    public int minRopeListSize = 5;

    [SerializeField]
    private float lineWidth = 0.1f;

    [SerializeField]
    float gravityScale = 2.0f;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        segmentLength = 0.1f;

        if(Info.partner == null)
            return;

        // Get the line renderer
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = transform.position; // Start at player position

        // Calculate the number of segments
        distance = Vector2.Distance(Info.player.transform.position, Info.partner.transform.position);
        currRopeListSize = (int)(distance / segmentLength);

        // Create and add segments to list
        for (int i = 0; i < maxRopeListSize; ++i)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= segmentLength; // Avoid overlap
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // FIXED UPDATE ========================================================
    private void FixedUpdate()
    {
        if (Info.partner == null)
            return;

        // Update then draw
        // ORDER MATTERS!!!
        Simulate();
        DrawRope();
    }


    ////////////////////////////////////////////////////////////////////////
    // SIMULATE ============================================================
    // Simulate physics/gravity for each rope segment
    private void Simulate()
    {
        Vector2 forceGravity = new Vector2(0.0f, -gravityScale);

        ApplyConstraint(); // Ensure proper rope setup before applying physics

        for (int i = 0; i < currRopeListSize; ++i) 
        {
            RopeSegment currSegment = ropeSegments[i];
            Vector2 velocity = currSegment.posNow - currSegment.posOld;
            currSegment.posOld = currSegment.posNow;
            currSegment.posNow += velocity;
            currSegment.posNow += forceGravity * Time.fixedDeltaTime;
            ropeSegments[i] = currSegment;
        }
        ApplyConstraint(); // Ensure physics didn't detach rope or move it past constraints
    }


    ////////////////////////////////////////////////////////////////////////
    // APPLY CONSTRAINT ====================================================
    // Apply physical contraints to the rope, to keep each end tethered to
    // the partner/player
    private void ApplyConstraint2()
    {
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = Info.player.transform.position;
        ropeSegments[0] = firstSegment;

        RopeSegment endSegment = ropeSegments[currRopeListSize -1];
        endSegment.posNow = Info.partner.transform.position;
        ropeSegments[currRopeListSize -1] = endSegment;

        float totalXdist = firstSegment.posNow.x - endSegment.posNow.x;
        float totalYdist = firstSegment.posNow.y - endSegment.posNow.y;

        // Total up the distances so we can compute an overall error factor and then apply uniformly
        float[] distances = new float[currRopeListSize];
        float totalDistance = 0;

        for (int i = 0; i < currRopeListSize - 1; ++i)
        {
            float dist = (ropeSegments[i].posNow - ropeSegments[i + 1].posNow).magnitude;
            distances[i] = dist;
            totalDistance += dist;
        }

        float totalError = totalDistance - segmentLength * currRopeListSize;
        if (totalDistance != 0 && totalError != 0)
        {
            float adjustment = 1 / (totalError - totalDistance);

            int midPoint = (currRopeListSize - 1) / 2;

            float baseX = firstSegment.posNow.x;
            float baseY = firstSegment.posNow.y;
            for (int i = 1; i < midPoint; ++i)
            {
                RopeSegment currSegment = ropeSegments[i];
                //RopeSegment nextSegment = ropeSegments[i + 1];

                float newX = (currSegment.posNow.x - baseX) * adjustment + baseX; // approx...
                float newY = (currSegment.posNow.y - baseY) * adjustment + baseY; // approx...

                currSegment.posNow.x = newX;
                currSegment.posNow.y = newY;
                ropeSegments[i] = currSegment;
            }
            baseY = endSegment.posNow.y;
            for (int i = midPoint; i < currRopeListSize - 1; ++i)
            {
                RopeSegment currSegment = ropeSegments[i];
                //RopeSegment nextSegment = ropeSegments[i + 1];

                float newX = (currSegment.posNow.x - baseX) * adjustment + baseX; // approx...
                float newY = (currSegment.posNow.y - baseY) * adjustment + baseY; // approx...

                currSegment.posNow.x = newX;
                currSegment.posNow.y = newY;
                ropeSegments[i] = currSegment;
            }
        }

        CheckConstraint(); // debug
    }

    private void ApplyConstraint()
    {
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = Info.player.transform.position;
        ropeSegments[0] = firstSegment;

        RopeSegment endSegment = ropeSegments[currRopeListSize -1];
        endSegment.posNow = Info.partner.transform.position;
        ropeSegments[currRopeListSize -1] = endSegment;

        for (int i = 0; i < currRopeListSize - 1; ++i)
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
        CheckConstraint(); // debug
    }

    private void CheckConstraint()
    {
        RopeSegment firstSegment = ropeSegments[0];
        if (firstSegment.posNow != (Vector2)Info.player.transform.position)
            Debug.LogWarning($"First segment not positioned on player");

        RopeSegment endSegment = ropeSegments[currRopeListSize -1];
        endSegment.posNow = Info.partner.transform.position;
        if (endSegment.posNow != (Vector2)Info.partner.transform.position)
            Debug.LogWarning($"End segment not positioned on partner");

        for (int i = 0; i < currRopeListSize - 1; ++i)
        {
            RopeSegment currSegment = ropeSegments[i];
            RopeSegment nextSegment = ropeSegments[i + 1];

            float dist = (currSegment.posNow - nextSegment.posNow).magnitude;
            float error = Mathf.Abs(dist - segmentLength);

            if (error >= 0.05 || error <= -0.05)
                Debug.LogWarning($"Segment {i} length {dist} not segment length {segmentLength} (error: {error}");
        }
        Debug.Log($"Checked {currRopeListSize} rope segments");
    }

    ////////////////////////////////////////////////////////////////////////
    // DRAW ROPE ===============================================================
    private void DrawRope()
    {
        float width = lineWidth;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        Vector3[] ropePositions = new Vector3[maxRopeListSize];
        
        for (int i = 0; i < currRopeListSize; ++i)
            ropePositions[i] = ropeSegments[i].posNow;

        lineRenderer.positionCount = currRopeListSize;
        lineRenderer.SetPositions(ropePositions);
    }
}
