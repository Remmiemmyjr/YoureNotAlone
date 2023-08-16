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
    private bool allowMovement = false;

    public float speed = 3f;

    public Transform[] wayPoints;
    public int startingPoint;

    int i;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        if (startOnAwake)
        {
            allowMovement = true;
        }

        transform.position = wayPoints[startingPoint].position;
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        if (allowMovement)
        {
            if (Vector2.Distance(transform.position, wayPoints[i].position) < 0.02f)
            {
                i++;

                if (i == wayPoints.Length)
                {
                    i = 0;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, wayPoints[i].position, speed * Time.deltaTime);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // COLLISION ENTER =====================================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Partner")
        {
            collision.transform.SetParent(transform);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // COLLISION EXIT ======================================================
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Partner")
        {
            collision.transform.SetParent(null);
        }
    }
}
