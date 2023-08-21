//*************************************************
// Project: We're Tethered Together
// File: MovingPlatform.cs
// Author/s: Emmy Berg
//           Corbyn LaMar
//
// Desc: Moving platform that can travel to
//       mulitple waypoints
//
// Last Edit: 8/11/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    [SerializeField]
    private bool startOnAwake = false;

    [SerializeField]
    private bool loopPoints = false;
    private bool canLoop = true;

    private bool allowMovement = false;

    private LineRenderer lineRend;

    public float speed = 3f;

    private Transform[] wayPointsGizmo;
    private Transform[] wayPoints;
    public int startingPoint;

    int i;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
    }

    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        // Get all of the waypoints from the children of the parent of this Gameobject
        Transform wpContainer = transform.parent.Find("WayPoints");
        int waypointCount = wpContainer.childCount;

        wayPoints = new Transform[waypointCount];

        for (int i = 0; i < waypointCount; i++)
        {
            wayPoints[i] = wpContainer.GetChild(i);
        }

        // Other stuff
        if (startOnAwake)
        {
            allowMovement = true;
        }

        transform.position = wayPoints[startingPoint].position;

        lineRend.positionCount = wayPoints.Length;
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        // Draw the line
        for(int i = 0; i < wayPoints.Length; ++i)
        {
            lineRend.SetPosition(i, wayPoints[i].position);
        }

        if (allowMovement && canLoop)
        {
            if (Vector2.Distance(transform.position, wayPoints[i].position) < 0.02f)
            {
                i++;

                if (i == wayPoints.Length)
                {
                    i = 0;

                    if(!loopPoints)
                        canLoop = false;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, wayPoints[i].position, speed * Time.deltaTime);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // COLLISION ENTER =====================================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Info.isDead)
            return;

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Partner")
        {
            if (!startOnAwake)
                allowMovement = true;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // COLLISION STAY ======================================================
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Info.isDead)
            return;

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Grabbable")
        {
            collision.transform.SetParent(transform);
        }

        if(collision.gameObject.tag == "Partner")
        {
            if(Info.partner.GetComponentInChildren<Latch>().isLatched)
            {
                collision.transform.SetParent(null);
            }
            else
            {
                collision.transform.SetParent(transform);
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // COLLISION EXIT ======================================================
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (Info.isDead)
            return;

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Partner" || collision.gameObject.tag == "Grabbable")
        {
            collision.transform.SetParent(null);
        }
    }

    void OnDrawGizmos()
    {
        // Get all of the waypoints from the children of the parent of this Gameobject
        Transform wpContainer = transform.parent.Find("WayPoints");
        int waypointCount = wpContainer.childCount;

        wayPointsGizmo = new Transform[waypointCount];

        for (int i = 0; i < waypointCount; i++)
        {
            wayPointsGizmo[i] = wpContainer.GetChild(i);
        }

        Gizmos.color = Color.blue;

        for (int i = 0; i < wayPointsGizmo.Length - 1; ++i)
        {
            Gizmos.DrawLine(wayPointsGizmo[i].position, wayPointsGizmo[i + 1].position);
        }
    }
}
