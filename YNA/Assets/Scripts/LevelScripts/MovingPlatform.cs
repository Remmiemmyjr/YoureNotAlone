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

    public Transform[] wayPoints;
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
        Gizmos.color = Color.blue;

        for (int i = 0; i < wayPoints.Length - 1; ++i)
        {
            Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
        }
    }
}
