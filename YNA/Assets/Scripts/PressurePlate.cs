//*************************************************
// Project: We're Tethered Together
// File: PressurePlate.cs
// Author/s: Emmy Berg
//
// Desc: Manage player actions
//
// Notes:
//  + Door speed is inconsistent in executable
//
// Last Edit: 5/3/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public GameObject door;

    Vector2 ogPosDoor;

    Color ogColor;

    bool timeToReturn = false;
    bool onPlate;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        ogColor = gameObject.GetComponent<SpriteRenderer>().color;
        ogPosDoor = door.GetComponent<Transform>().position;
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        if(timeToReturn)
        {
            if(door.GetComponent<Transform>().position.y > ogPosDoor.y)
            {
                door.GetComponent<Transform>().Translate(0, -7f * Time.deltaTime, 0);
            }
            else
            {
                timeToReturn = false;
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        onPlate = true;
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER STAY ========================================================
    private void OnTriggerStay2D(Collider2D collision)
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.35f, 0.51f, 0.39f);

        timeToReturn = false;
        if (door.GetComponent<Transform>().position.y < ogPosDoor.y + door.GetComponent<Transform>().localScale.y + 2)
        {
            door.GetComponent<Transform>().Translate(0, 7f * Time.deltaTime, 0);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER STAY ========================================================
    private void OnTriggerExit2D(Collider2D collision)
    {
        onPlate = false;
        timeToReturn = true;
        gameObject.GetComponent<SpriteRenderer>().color = ogColor;
    }
}
