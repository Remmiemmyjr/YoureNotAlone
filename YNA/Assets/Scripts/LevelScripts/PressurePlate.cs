//*************************************************
// Project: We're Tethered Together
// File: PressurePlate.cs
// Author/s: Emmy Berg
//           Corbyn LaMar
//
// Desc: Open and close assigned doors via plate
//       activation
//
// Notes:
// -
//
// Last Edit: 7/16/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
////////////////////////////////////////////////////////////////////////
// VARIABLES ===========================================================
    Vector2 ogPosDoor;
    Color ogColor;

    public GameObject door;

    [SerializeField]
    private bool isHorizontal = false;
    bool timeToReturn = false;
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
            if(!isHorizontal && door.GetComponent<Transform>().position.y > ogPosDoor.y)
            {
                door.GetComponent<Transform>().Translate(0, -7f * Time.deltaTime, 0);
            }
            else if(isHorizontal && door.GetComponent<Transform>().position.x > ogPosDoor.x)
            {
                door.GetComponent<Transform>().Translate(0, 7f * Time.deltaTime, 0);
            }
            else
            {
                timeToReturn = false;
            }
        }
    }


////////////////////////////////////////////////////////////////////////
// TRIGGER STAY ========================================================
    private void OnTriggerStay2D(Collider2D collision)
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.35f, 0.51f, 0.39f);

        timeToReturn = false;
        if (!isHorizontal && door.GetComponent<Transform>().position.y < ogPosDoor.y + ((door.GetComponent<Transform>().localScale.y * 2.0f) - (door.GetComponent<Transform>().localScale.y * 0.2f)))
        {
            door.GetComponent<Transform>().Translate(0, 7f * Time.deltaTime, 0);
        }
        else if (isHorizontal && door.GetComponent<Transform>().position.x < ogPosDoor.x + ((door.GetComponent<Transform>().localScale.y * 2.0f) - (door.GetComponent<Transform>().localScale.y * 0.2f)))
        {
            door.GetComponent<Transform>().Translate(0, -7f * Time.deltaTime, 0);
        }
    }


////////////////////////////////////////////////////////////////////////
// TRIGGER EXIT ========================================================
    private void OnTriggerExit2D(Collider2D collision)
    {
        timeToReturn = true;
        gameObject.GetComponent<SpriteRenderer>().color = ogColor;
    }
}
