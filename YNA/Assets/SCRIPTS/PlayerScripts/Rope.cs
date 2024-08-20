//*************************************************
// Project: We're Tethered Together
// File: Rope.cs
// Author/s: Emmy Berg
//           Corbyn LaMar
//
// Desc: Manage player actions
//
// Notes:
//  -
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
        maxRopeListSize = (int)(Info.grapple.maxRopeLimit / segmentLength);
        segmentLength = 0.1f;

        if (Info.partner == null)
            return;

        // Get the line renderer
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = transform.position; // Start at player position

        // Calculate the number of segments
        distance = Vector2.Distance(Info.player.transform.position, Info.partner.transform.position);
        currRopeListSize = (int)(distance / segmentLength);

        // Make sure we don't go over max
        if (currRopeListSize > maxRopeListSize)
        {
            maxRopeListSize = currRopeListSize;
        }

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


    private void AttachTether(out RopeSegment firstSegment, out RopeSegment endSegment)
    {
        // hookup start and end of rope
        firstSegment = ropeSegments[0];
        firstSegment.posNow = Info.player.transform.position;
        ropeSegments[0] = firstSegment;

        endSegment = ropeSegments[currRopeListSize - 1];
        endSegment.posNow = Info.partner.transform.position;
        ropeSegments[currRopeListSize - 1] = endSegment;

        // Sometimes rope gets messed up (like in respawn, check for it...)
        bool needFixup = false;
        for (int i = 0; i < currRopeListSize; ++i)
        {
            RopeSegment currSegment = ropeSegments[i];
            if (float.IsNaN(currSegment.posNow.x) || float.IsInfinity(currSegment.posNow.x) || float.IsNaN(currSegment.posNow.y) || float.IsInfinity(currSegment.posNow.y))
            {
                Debug.LogWarning("Rope broken, repairing...");
                needFixup = true;
                break;
            }
        }

        if (!needFixup) return;

        // best to fixup entire rope, other points are probably close to going...
        //LineFormula ropeFixup = new LineFormula(firstSegment.posNow, endSegment.posNow);
        float distPlayerPartner = Mathf.Max((firstSegment.posNow - endSegment.posNow).magnitude, 0.5f); // if we're too close, assume a minimum distance
        float forceSegmentLength = distPlayerPartner / currRopeListSize;

        float baseX = firstSegment.posNow.x;
        for (int i = 1; i < currRopeListSize - 1; ++i)
        {
            RopeSegment currSegment = ropeSegments[i];
            currSegment.posNow.x = baseX + i * forceSegmentLength;
            //currSegment.posNow.y = ropeFixup.Y(currSegment.posNow.x);
            currSegment.posOld = currSegment.posNow; // ensure no initial velocity
            ropeSegments[i] = currSegment;
        }
    }

    internal void ResetRope()
    {
        for (int i = 0; i < currRopeListSize; ++i)
        {
            RopeSegment currSegment = ropeSegments[i];
            currSegment.posOld = currSegment.posNow;
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // SIMULATE ============================================================
    // Simulate physics/gravity for each rope segment
    private void Simulate()
    {
        Vector2 forceGravity = new Vector2(0.0f, -gravityScale);

        for (int i = 0; i < currRopeListSize; ++i)
        {
            RopeSegment currSegment = ropeSegments[i];
            Vector2 velocity = currSegment.posNow - currSegment.posOld;
            currSegment.posOld = currSegment.posNow;
            currSegment.posNow += velocity;
            currSegment.posNow += forceGravity * Time.fixedDeltaTime;
            ropeSegments[i] = currSegment;
            ApplyConstraint();
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // APPLY CONSTRAINT ====================================================
    // Apply physical contraints to the rope, to keep each end tethered to
    // the partner/player
    private void ApplyConstraint()
    {
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = Info.player.transform.position;
        //firstSegment.posOld = firstSegment.posNow;
        ropeSegments[0] = firstSegment;

        RopeSegment endSegment = ropeSegments[currRopeListSize - 1];
        endSegment.posNow = Info.partner.transform.position;
        //endSegment.posOld = endSegment.posNow;
        ropeSegments[currRopeListSize - 1] = endSegment;

        AttachTether(out firstSegment, out endSegment);

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
