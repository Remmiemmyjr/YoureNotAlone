//*************************************************
// Project: We're Tethered Together
// File: Rope.cs
// Author/s: Emmy Berg
//           Mike Doeren
//           Corbyn LaMar
//           David Berg
//
// Desc: Manage player actions
//
// Notes:
// -
//
// Last Edit: 8/24/2023
//
//*************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Implements a rope that hangs between the player and partner (when connected)
/// </summary>
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

    //[HideInInspector]
    public float segmentLength = 0.2f; // 0.1 is too close...;
    [HideInInspector]
    public int currRopeListSize = 0;
    //[HideInInspector]
    public int maxRopeListSize = 70;
    [HideInInspector]
    public int minRopeListSize = 5;

    [SerializeField]
    private float lineWidth = 0.1f;

    [SerializeField]
    [Tooltip("Amount of gravity applied to rope segments.  0 = no gravity (rope floats), 2 seems to work well.")]
    [Min(0)]
    float gravityScale = 2.0f;

    [SerializeField]
    [Tooltip("Amount of friction applied to rope segments to slow them down (0+).  0 = no friction, higher values contribute greater friction...")]
    [Min(0)]
    float frictionFactor = 2.0f;  // 0 = no friction, > 0 = friction, < 0 = accelerate (higher friction reduces wobble and vibration)

    [SerializeField]
    [Tooltip("Amount of compression applied to rope segments when player and partner get closer (0-1).  0 = no compression (long rope hanging down), 1 = full compression (rope stays no matter how close they get), 0.5 = rope hangs down half as much as it would.")]
    [Range(0,1)]
    float CompressionFactor = 1.0f; // 0 = no rope compression as players get closer, 1 = full rope compression as players get closer (0.5 = 50%)

    [SerializeField]
    [Tooltip("Amount of force applied to pull rope toward the ends (0+).  Counterbalances the pull of gravity and keeps it from stretching rope way out.  Higher for straighter, lower for hanging down more. 40 seems to be a good value.")]
    [Min(0)]
    float ropePullForceMultiplier = 40f; // Higher numbers counterbalance gravity = straighter rope, lower numbers let gravity have more impact = stretching and sinking rope

    float extraDistanceFactor = 1.5f; // amount of extra distance to build into rope (so it's not straight across) - no longer used, part of old constraint system.    // *********************************************************************

    [Tooltip("Turn on debug logging for the rope.")]
    public bool DebugLogging = false;

    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        //segmentLength = 0.2f; // 0.1 is too close...

        if(Info.partner == null) // too soon!
            return;

        // Get the line renderer
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = transform.position; // Start at player position

        // Create and add segments to list
        for (int i = 0; i < maxRopeListSize; ++i)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.x -= segmentLength; // Avoid overlap
            ropeStartPoint.y -= segmentLength; // Avoid overlap
        }

        // Calculate the number of segments to use
        currRopeListSize = 0;
        distance = Vector2.Distance(Info.player.transform.position, Info.partner.transform.position); // what if partner not attached?
        SetSize(distance);
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
    //  ---------------------------------
    // Earlier versions tried to separate out the constraints from the physics.  That didn't work well (see details on old methods below).
    // It seems to work much better to apply the constraints and physics at the same time.
    // The current system only applies one constraint up front: that the rope is attached and valid (handled by AttachTether),
    // everything else is applied through self balancing forces based on constraints rather than physical constraints.
    // The result is a very stretchy and well behaved rope that moves smoothly toward a stable, balanced point (although that can be tweaked through varous parameters to the point where the rope is no longer stable).
    private void Simulate()
    {
        if (!Ready()) return;
        RopeSegment firstSegment, endSegment;
        AttachTether(out firstSegment, out endSegment);

        Vector2 forceGravity = new Vector2(0.0f, -gravityScale); // /4.0f);
        float distPlayerPartner = Mathf.Max((firstSegment.posNow - endSegment.posNow).magnitude, 0.5f); // if we're too close, assume a minimum distance
        // Rather than using the real segment length, we force one based on the distance to partner.  This creates a more balanced set of forces on the rope and keeps it from hanging ridiculously low when the move close to each other.
        float forceSegmentLength = distPlayerPartner / currRopeListSize;
        if (forceSegmentLength < segmentLength && CompressionFactor >= 0 && CompressionFactor < 1)
        {
            forceSegmentLength += (segmentLength - forceSegmentLength) * (1 - CompressionFactor);
        }
        //LineFormula ropeFixup = null;

        for (int i = 0; i < currRopeListSize; ++i)
        {
            RopeSegment currSegment = ropeSegments[i];
            //if (float.IsNaN(currSegment.posNow.x)|| float.IsInfinity(currSegment.posNow.x) ||float.IsNaN(currSegment.posNow.y)|| float.IsInfinity(currSegment.posNow.y))
            //{
            //    // Sometimes rope gets messed up (like in respawn, so just fix it up...)
            //    if (ropeFixup == null)
            //    {
            //        ropeFixup = new LineFormula(firstSegment.posNow, endSegment.posNow);
            //    }
            //    currSegment.posNow.x = i * forceSegmentLength;
            //    currSegment.posNow.y = ropeFixup.Y(currSegment.posNow.x);
            //}
            Vector2 velocity = (currSegment.posNow - currSegment.posOld) / (1f + frictionFactor);  // divide for friction and debouncing...
            currSegment.posOld = currSegment.posNow;
            if (i == 0) // First Rope Segment is locked to Player
            {
                ropeSegments[i] = currSegment;
                if (DebugLogging) Debug.Log($"Segment {i}: player: {currSegment.posOld}");
                continue;
            }
            if (i == currRopeListSize-1) // End Rope Segment is locked to Partner
            {
                ropeSegments[i] = currSegment;
                if (DebugLogging) Debug.Log($"Segment {i}: partner: {currSegment.posOld}");
                continue;
            }

            currSegment.posNow += velocity; // Apply old velocity (reduced by friction, consider applying portion of velocity of adjacent sections...)

            Vector2 totalForce = forceGravity;
            Vector2 playerForce = Vector2.zero;
            Vector2 partnerForce = Vector2.zero;

            float playerDist = (firstSegment.posNow - currSegment.posNow).magnitude; // how far is the player from this segment
            float maxPlayerDist = (i - 1) * forceSegmentLength; // If we're closer than our stretchd out distance, then there's no rope pull force applied
            if (playerDist > maxPlayerDist)
            {
                playerForce = (firstSegment.posNow - currSegment.posNow) * (playerDist - maxPlayerDist) * ropePullForceMultiplier / i; // create force to pull back toward player if we exceed partial rope distance
                totalForce += playerForce;
            }

            float partnerDist= (endSegment.posNow - currSegment.posNow).magnitude;
            float maxPartnerDist = (currRopeListSize - i + 0) * forceSegmentLength;
            if (partnerDist > maxPartnerDist)
            {
                partnerForce = (endSegment.posNow - currSegment.posNow) * (partnerDist - maxPartnerDist) * ropePullForceMultiplier / (currRopeListSize - i); // create force to pull back toward partner if we exceed partial rope distance
                totalForce += partnerForce;
            }

            currSegment.posNow += totalForce * Time.fixedDeltaTime;
            ropeSegments[i] = currSegment;
            if (DebugLogging) Debug.Log($"Segment {i}: old: {currSegment.posOld} + Velocity {velocity} + Total Force {totalForce} (Gravity {forceGravity} + Player {playerForce} + Partner {partnerForce}) -> new {currSegment.posNow}");
        }
        if (DebugLogging) CheckConstraint(); // output any issues, point list
    }

    // Prior version of the rope physics simulation.
    // First version to attempt to use balanced forces, but got thrown off by use of squared magnitude instead of actual magnitude.  
    // After fiddling with this for a while, I decided to take the hit of computing actual magnitude (requires a square root)
    // So I set this aside and started on the next (latest) version.
    private void Simulate2()
    {
        if (!Ready()) return;
        RopeSegment firstSegment, endSegment;
        AttachTether(out firstSegment, out endSegment);

        Vector2 forceGravity = new Vector2(0.0f, -gravityScale);

        //ApplyConstraint(); // Ensure proper rope setup before applying physics

        float segmentLengthSqr = segmentLength * segmentLength;

        for (int i = 0; i < currRopeListSize; ++i)
        {
            RopeSegment currSegment = ropeSegments[i];
            Vector2 velocity = currSegment.posNow - currSegment.posOld;
            currSegment.posOld = currSegment.posNow;
            if (i == 0)
            {
                ropeSegments[i] = currSegment;
                Debug.Log($"Segment {i}: player: {currSegment.posOld}");
                continue;
            }
            if (i == currRopeListSize-1)
            {
                ropeSegments[i] = currSegment;
                Debug.Log($"Segment {i}: partner: {currSegment.posOld}");
                continue;
            }
            currSegment.posNow += velocity;

            Vector2 totalForce = forceGravity;
            Vector2 playerForce = Vector2.zero;
            Vector2 partnerForce = Vector2.zero;

            float playerDistSqr = (firstSegment.posNow - currSegment.posNow).sqrMagnitude;
            float maxPlayerDistSqr = i * i * segmentLengthSqr;
            if (playerDistSqr > maxPlayerDistSqr)
            {
                playerForce = (firstSegment.posNow - currSegment.posNow) * (playerDistSqr - maxPlayerDistSqr); // create force to pull back toward player if we exceed partial rope distance
                totalForce += playerForce;
            }

            float partnerDistSqr = (endSegment.posNow - currSegment.posNow).sqrMagnitude;
            float maxPartnerDistSqr = (currRopeListSize - i) * (currRopeListSize - i) * segmentLengthSqr;
            if (partnerDistSqr > maxPartnerDistSqr)
            {
                partnerForce = (endSegment.posNow - currSegment.posNow) * (partnerDistSqr - maxPartnerDistSqr); // create force to pull back toward partner if we exceed partial rope distance
                totalForce += partnerForce;
            }

            currSegment.posNow += totalForce * Time.fixedDeltaTime;
            ropeSegments[i] = currSegment;
            if (DebugLogging) Debug.Log($"Segment {i}: old: {currSegment.posOld} + Velocity {velocity} + Total Force {totalForce} (Gravity {forceGravity} + Player {playerForce} + Partner {partnerForce}) -> new {currSegment.posNow}");
        }
        //ApplyConstraint(); // Ensure physics didn't detach rope or move it past constraints
        if (DebugLogging) CheckConstraint(); // output any issues, point list
    }

    /// <summary>
    /// Attaches the ends of the rope to the player and partner and validates all the intermediate positions are valid.
    /// </summary>
    /// <param name="firstSegment">returns the first (player attached) segment</param>
    /// <param name="endSegment">returns the last (partner attached) segment</param>
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
                if (DebugLogging) Debug.LogWarning("Rope broken, repairing...");
                needFixup = true;
                break;
            }
        }

        if (!needFixup) return;

        // best to fixup entire rope, other points are probably close to going...
        LineFormula ropeFixup = new LineFormula(firstSegment.posNow, endSegment.posNow);
        float distPlayerPartner = Mathf.Max((firstSegment.posNow - endSegment.posNow).magnitude,0.5f); // if we're too close, assume a minimum distance
        float forceSegmentLength = distPlayerPartner / currRopeListSize;

        float baseX = firstSegment.posNow.x;
        for (int i = 1; i < currRopeListSize-1; ++i)
        {
            RopeSegment currSegment = ropeSegments[i];
            currSegment.posNow.x = baseX + i * forceSegmentLength;
            currSegment.posNow.y = ropeFixup.Y(currSegment.posNow.x);
            currSegment.posOld = currSegment.posNow; // ensure no initial velocity
            ropeSegments[i] = currSegment;
        }
    }

    /// <summary>
    /// Sets the size of the rope.  Moved here from Grapple to clean it up and improve isolation.
    /// Key factor was making sure to set both old and new points to avoid adding random velocity to the rope segments.
    /// </summary>
    /// <param name="currMaxRopeLimit">The maximum length we want to set the rope to.</param>
    internal void SetSize(float currMaxRopeLimit)
    {
        int newRopeListSize = (int)(currMaxRopeLimit / segmentLength);

        if (!Ready(newRopeListSize)) return;

        if (newRopeListSize > maxRopeListSize)
            newRopeListSize = maxRopeListSize;

        else if (newRopeListSize < minRopeListSize)
            newRopeListSize = minRopeListSize;

        if (currRopeListSize != newRopeListSize)
        {
            int nextSegmentOffset = Math.Max(currRopeListSize, 1);
            currRopeListSize = newRopeListSize;

            RopeSegment firstSegment, endSegment;
            AttachTether(out firstSegment, out endSegment);
            //float distance = Vector2.Distance(Info.player.transform.position, Info.partner.transform.position);
            float totalXdist = firstSegment.posNow.x - endSegment.posNow.x;
            float segmentStartingLength = totalXdist / newRopeListSize;
            LineFormula segmentLine = new LineFormula(ropeSegments[nextSegmentOffset].posNow, endSegment.posNow);

            for (int segment = nextSegmentOffset; segment < newRopeListSize-1; segment++) // pay out the line
            {
                RopeSegment newSegment = ropeSegments[segment];
                newSegment.posNow.x = ropeSegments[segment - 1].posNow.x + segmentStartingLength;
                newSegment.posNow.y = segmentLine.Y(newSegment.posNow.x);
                newSegment.posOld = newSegment.posNow; // otherwise we give it a strong initial velocity which triggers the wiggles...
                ropeSegments[segment] = newSegment;
            }
        }
    }

    /// <summary>
    /// Used to check if the rope is attached and a valid size before we start trying to manipulate it.
    /// </summary>
    /// <param name="newRopeListSize"></param>
    /// <returns></returns>
    private bool Ready(int newRopeListSize = -1)
    {
        if (Info.partner == null) return false;

        if (newRopeListSize == -1) newRopeListSize = currRopeListSize;
        return ropeSegments.Count > newRopeListSize && newRopeListSize > 0;
    }

    /// <summary>
    /// First version of physics simulation.  This applies gravity and inertia (maintains velocity)
    /// Problems were the lack of friction so that rope segments could wiggle for a long time,
    /// and trying to apply positional constraints before/after (and independently of) the forces: simulate would move the rope into a new position, 
    /// then applyconstraint would try to move them back (undoing what simulate just did).  Keeping them in sync was painful and didn't work well.
    /// </summary>
    private void Simulate1()
    {
        Vector2 forceGravity = new Vector2(0.0f, gravityScale);

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

    /// <summary>
    /// At one point I was trying to apply constraints by forcing points into a parabolic shape.  
    /// This class does the work of figuring out a matching parabola formula and then applying it.
    /// I started trying to use 3 points to define the parabola.  That worked, but the third point was hard to calculate 
    /// (I wanted it to approximate the vertex, but it would be too far off and the parabola would hang down further than I expected.)
    /// 
    /// Then I realized I'd be better off with 2 points and a line to target the vertex.  I wrote that, but never tested it because by then I'd decided the forces approach was better.
    /// I thought this might still be useful, so I added code to support other setups.
    /// </summary>
    public class ParabolaFormula
    {
        public float a, b, c;

        /// <summary>
        /// Create a Parabola Formula based on three points.
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <param name="p3">Third point</param>
        public ParabolaFormula(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            // Calculate the coefficients a, b, and c using the provided points
            float x1 = p1.x;
            float y1 = p1.y;
            float x2 = p2.x;
            float y2 = p2.y;
            float x3 = p3.x;
            float y3 = p3.y;

            // Use Cramer's rule to solve for a, b, and c
            //float determinant = x1 * (x2 - x3) - x2 * (x1 - x3) + x3 * (x1 - x2);
            float determinant = x1 * (x2 * x2 - x3 * x3) - x2 * (x1 * x1 - x3 * x3) + x3 * (x1 * x1 - x2 * x2);

            if (Mathf.Abs(determinant) < float.Epsilon)
            {
                //throw new ArgumentException("The points are collinear, and a unique parabola cannot be determined.");
                Debug.Log($"The points {p1},{p2},{p3} are collinear, and a unique parabola cannot be determined.  Defaulting to a line.");
                a = 0; // Liner, a is 0
                if (x2-x1 == 0)
                {
                    Debug.Log($"X1 and X2 are the same, the line is vertical or undeterminable.");
                    b = 0;
                    c = 0;
                    return;
                }
                b = (y2 - y1) / (x2 - x1);  // Calculate the slope (m -> b)
                c = y1 - b * x1;  // Calculate the y-intercept (b -> c) using the slope and one of the points
                return;
            }

            a = ((y1 - y2) * (x2 - x3) - (y2 - y3) * (x1 - x2)) / determinant;
            b = ((y1 - y2) * (x3 * x3 - x2 * x2) - (y2 - y3) * (x1 * x1 - x2 * x2)) / determinant;
            c = y1 - a * x1 * x1 - b * x1;
            Debug.Log($"The points {p1},{p2},{p3} yielded parabolic equation {a}x^2 + {b}x + {c}.");
        }

        /// <summary>
        /// Create a Parabola Formula based on two points and a tangent line
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <param name="slope">Slope of the line</param>
        /// <param name="yIntercept">Y intercept of the line</param>
        public ParabolaFormula(Vector2 p1, Vector2 p2, float slope, float yIntercept)
        {
            // Calculate the coefficients a, b, and c using the provided points
            float x1 = p1.x;
            float y1 = p1.y;
            float x2 = p2.x;
            float y2 = p2.y;

            a = (y1 - yIntercept - slope * x1 * x1 - slope * x2 * x2) / (x1 * x1 - 2 * x1 * x2 + x2 * x2);
            b = slope * (x1 + x2) - 2 * a * x1 - 2 * a * x2;
            c = y1 - a * x1 * x1 - b * x1;
            Debug.Log($"The points {p1} and {p2} and line y={slope}x + {yIntercept} yielded parabolic equation {a}x^2 + {b}x + {c}.");
        }

        /// <summary>
        /// Create a Parabola Formula object from it's coefficients (y = ax^2 + bx + c)
        /// </summary>
        /// <param name="a">First coefficient (ax^2)</param>
        /// <param name="b">Second coefficient (bx)</param>
        /// <param name="c">Third coefficient (c = constant)</param>
        public ParabolaFormula(float a, float b, float c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        /// <summary>
        /// Create a Parabola Formula object from it's vertex and scale (y = a(x-h)^2 + k, where (h,k) is the vertex and a is the scale).
        /// Then convert to standard form by multiplying: y = ax^2 - 2ahx + ah^2 + k
        /// </summary>
        /// <param name="a">scale(ax^2)</param>
        /// <param name="vertex">vertex (h,k))</param>
        public ParabolaFormula(float a, Vector2 vertex)
        {
            this.a = a;
            this.b = -2 * a * vertex.x; // h = vertex.x
            this.c = a * vertex.x * vertex.x + vertex.y; // k = vertex.y
            Debug.Log($"The scale {a} and vertex {vertex} yielded parabolic equation {a}x^2 + {b}x + {c}.");
        }

        public float Y(float X)
        {
            return a * X * X + b * X + c;
        }
    }

    /// <summary>
    /// Helper class to find and utilize the formula for a line.  
    /// </summary>
    public class LineFormula
    {
        public float m, b;

        public LineFormula(Vector2 p1, Vector2 p2)
        {
            // Calculate the slope m and y-intercept b using the provided points
            float x1 = p1.x;
            float y1 = p1.y;
            float x2 = p2.x;
            float y2 = p2.y;

            if (x2-x1 == 0)
            {
                Debug.Log($"X1 and X2 are the same, the line is vertical or undeterminable.");
                m = 0;
                b = 0;
                return;
            }
            m = (y2 - y1) / (x2 - x1);  // Calculate the slope (m)
            b = y1 - m * x1;  // Calculate the y-intercept (b) using the slope and one of the points
        }

        public LineFormula(float m, float b)
        {
            this.m = m;
            this.m = b;
        }

        public float Y(float X)
        {
            return m * X + b;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // APPLY CONSTRAINT ====================================================
    // Apply physical contraints to the rope, to keep each end tethered to
    // the partner/player
    // This version also tried to fit it to the parabola, which almost worked, but was ultiately discarded for the balanced force approach.
    private void ApplyConstraintP()
    {
        // Parabolic model

        RopeSegment firstSegment, endSegment;
        AttachTether(out firstSegment, out endSegment);

        float totalXdist = firstSegment.posNow.x - endSegment.posNow.x;
        float partnerDist = (Info.player.transform.position - Info.partner.transform.position).magnitude;

        // Inverted Y axis, lower values are higher on the screen...
        Vector2 midPoint = new Vector2((endSegment.posNow.x + firstSegment.posNow.x) / 2, MathF.Max(endSegment.posNow.y, firstSegment.posNow.y) + 0.5f);

        ParabolaFormula lowerLimit = new ParabolaFormula(firstSegment.posNow, midPoint, endSegment.posNow); // lowest point for the rope to sink to
        LineFormula upperLimit = new LineFormula(firstSegment.posNow, endSegment.posNow); // highest point for the rope

        //Debug.Log($"Adjusting {currRopeListSize} rope segments (partner distance = {partnerDist}, target length = {targetRopeLength}, actual = {totalDistance} ({totalXdist},{totalYdist}), heightDelta = {heightDelta}, player maxDeltaY = {maxDeltaY})");

        // Adjust X values (evenly distributed)
        // Adjust Y values between line and parabola
        float baseX = firstSegment.posNow.x;
        float deltaX = totalXdist / currRopeListSize;
        for (int i = 1; i < currRopeListSize-1; ++i)
        {
            RopeSegment currSegment = ropeSegments[i];

            baseX -= deltaX;
            currSegment.posNow.x = baseX; 
            currSegment.posNow.y = Mathf.Clamp(currSegment.posNow.y, upperLimit.Y(baseX), lowerLimit.Y(baseX)); ;
            ropeSegments[i] = currSegment;
        }

        CheckConstraint(); // debug
    }

    // Earlier constraint method that tried to apply constraints independently of the rope physics... (didn't work well)
    private void ApplyConstraint()
    {
        // Linear model, doesn't really work well
        RopeSegment firstSegment, endSegment;
        AttachTether(out firstSegment, out endSegment);

        float totalXdist = firstSegment.posNow.x - endSegment.posNow.x;
        float totalYdist = firstSegment.posNow.y - endSegment.posNow.y;
        float partnerDist = (Info.player.transform.position - Info.partner.transform.position).magnitude;

        // Total up the distances so we can compute an overall error factor and then apply uniformly
        float[] distances = new float[currRopeListSize];
        float totalDistance = 0;

        for (int i = 0; i < currRopeListSize - 1; ++i)
        {
            float dist = (ropeSegments[i].posNow - ropeSegments[i + 1].posNow).magnitude;
            distances[i] = dist;
            totalDistance += dist;
        }

        var targetRopeLength = segmentLength * currRopeListSize;
        if (targetRopeLength  < (partnerDist * extraDistanceFactor)) // make sure the rope isn't too tight
        {
            targetRopeLength = (float)(partnerDist * extraDistanceFactor);
        }

        float totalError = totalDistance - targetRopeLength;
        if (totalDistance != 0 && totalError != 0)
        {
            //float adjustment = 1 / (totalError - totalDistance); - WRONG!
            //float adjustment = totalError / currRopeListSize / 2;

            int midPoint = (currRopeListSize - 1) / 2;
            float heightDelta = (firstSegment.posNow.y - endSegment.posNow.y); // allow for player and partner at different heights
            float maxDeltaY = Mathf.Max((targetRopeLength - heightDelta / extraDistanceFactor) / currRopeListSize / 2, 0.01f);

            if (DebugLogging) Debug.Log($"Adjusting {currRopeListSize} rope segments (partner distance = {partnerDist}, target length = {targetRopeLength}, actual = {totalDistance} ({totalXdist},{totalYdist}), heightDelta = {heightDelta}, player maxDeltaY = {maxDeltaY})");

            // Adjust X values (evenly distributed)
            float baseX = firstSegment.posNow.x;
            float deltaX = totalXdist / currRopeListSize;
            for (int i = 1; i < currRopeListSize; ++i)
            {
                RopeSegment currSegment = ropeSegments[i];
                //RopeSegment nextSegment = ropeSegments[i + 1];

                //float newX = (currSegment.posNow.x - baseX) * adjustment + baseX; // approx...
                //float newY = (currSegment.posNow.y - baseY) * adjustment + baseY; // approx...

                baseX -= deltaX;
                currSegment.posNow.x = baseX; //newX;
                //currSegment.posNow.y = newY;
                ropeSegments[i] = currSegment;
            }

            // adjust Y-values from player to midpoint of rope
            float baseY = firstSegment.posNow.y;
            for (int i = 1; i <= midPoint; ++i) // start at 1, don't adjust player attachment
            {
                RopeSegment currSegment = ropeSegments[i];
                //RopeSegment nextSegment = ropeSegments[i + 1];

                //float newX = (currSegment.posNow.x - baseX) * adjustment + baseX; // approx...
                //float newY = (currSegment.posNow.y - baseY) * adjustment + baseY; // approx...
                float newY = Mathf.Clamp(currSegment.posNow.y, baseY - maxDeltaY * i, baseY); // approx...
                //baseY -= maxDeltaY; // adjustment;

                //currSegment.posNow.x = newX;
                currSegment.posNow.y = newY;
                ropeSegments[i] = currSegment;
            }
            // adjust Y-values from partner to midpoint of rope
            baseY = endSegment.posNow.y;
            maxDeltaY = Mathf.Max((targetRopeLength * extraDistanceFactor + heightDelta) / currRopeListSize / 2, 0.01f);
            for (int i = currRopeListSize - 2; i >= midPoint; --i) // start at -2, don't adjust partner attachment
            {
                RopeSegment currSegment = ropeSegments[i];
                //RopeSegment nextSegment = ropeSegments[i + 1];

                //float newX = (currSegment.posNow.x - baseX) * adjustment + baseX; // approx...
                //float newY = (currSegment.posNow.y - baseY) * adjustment + baseY; // approx...
                float newY = Mathf.Clamp(currSegment.posNow.y, baseY - maxDeltaY * (currRopeListSize - i), baseY); // approx...
                //baseY -= maxDeltaY; // adjustment;

                //currSegment.posNow.x = newX;
                currSegment.posNow.y = newY;
                ropeSegments[i] = currSegment;
            }
        }

        CheckConstraint(); // debug
    }

    // Original veroin of constraints method.
    private void ApplyConstraint_Old()
    {
        RopeSegment firstSegment, endSegment;
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
        CheckConstraint(); // debug
    }

    // Debug method to ensure constraints are properly applied.  Mostly gave up on this...
    private void CheckConstraint()
    {
        RopeSegment firstSegment = ropeSegments[0];
        if (firstSegment.posNow != (Vector2)Info.player.transform.position)
            Debug.LogWarning($"First segment not positioned on player");

        RopeSegment endSegment = ropeSegments[currRopeListSize -1];
        endSegment.posNow = Info.partner.transform.position;
        if (endSegment.posNow != (Vector2)Info.partner.transform.position)
            Debug.LogWarning($"End segment not positioned on partner");

        StringBuilder sb = new StringBuilder();
        float totalDist = 0;
        for (int i = 0; i < currRopeListSize - 1; ++i)
        {
            RopeSegment currSegment = ropeSegments[i];
            RopeSegment nextSegment = ropeSegments[i + 1];

            float dist = (currSegment.posNow - nextSegment.posNow).magnitude;
            //float error = Mathf.Abs(dist - segmentLength);
            totalDist += dist;
            sb.Append('(').Append(currSegment.posNow.x).Append(',').Append(currSegment.posNow.y).Append(')').Append(dist);

            //if (error >= 0.05 || error <= -0.05)
            //    Debug.LogWarning($"Segment {i} length {dist} not segment length {segmentLength} (error: {error}");
        }
        sb.Append('(').Append(endSegment.posNow.x).Append(',').Append(endSegment.posNow.y).Append(')');
        Debug.Log($"Checked {currRopeListSize} rope segments (total distance = {totalDist}): {sb}");
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
