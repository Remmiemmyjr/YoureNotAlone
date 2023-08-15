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

    public Color activeColor;

    public GameObject door;
    public GameObject gem;
    public GameObject gemLight;

    SpriteRenderer gemSR;
    PuzzleDoorProfile doorProfile;

    [SerializeField]
    private bool isHorizontal = false;
    bool timeToReturn = false;
// *********************************************************************


////////////////////////////////////////////////////////////////////////
// START ===============================================================
    void Start()
    {
        ogPosDoor = door.GetComponent<Transform>().position;
        doorProfile = door.GetComponent<PuzzleDoorProfile>();
        door.GetComponent<SpriteRenderer>().color = activeColor;

        gemSR = gem.GetComponent<SpriteRenderer>();
        ogColor = gemSR.color;
        doorProfile.DisableGem(ogColor);
        
        gemLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color = activeColor;
        gemLight.SetActive(false);
    }


////////////////////////////////////////////////////////////////////////
// UPDATE ==============================================================
    void Update()
    {
        if(timeToReturn)
        {
            if(!isHorizontal && door.GetComponent<Transform>().position.y > ogPosDoor.y)
            {
                door.GetComponent<Transform>().Translate(0, -9f * Time.deltaTime, 0);
            }
            else if(isHorizontal && door.GetComponent<Transform>().position.x > ogPosDoor.x)
            {
                door.GetComponent<Transform>().Translate(0, 9f * Time.deltaTime, 0);
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
        gemSR.color = activeColor;
        gemLight.SetActive(true);
        doorProfile.EnableGem(activeColor);

        timeToReturn = false;
        if (!isHorizontal && door.GetComponent<Transform>().position.y < ogPosDoor.y + ((door.GetComponent<Transform>().localScale.y * 2.0f) - (door.GetComponent<Transform>().localScale.y * 0.2f)))
        {
            door.GetComponent<Transform>().Translate(0, 3f * Time.deltaTime, 0);
        }
        else if (isHorizontal && door.GetComponent<Transform>().position.x < ogPosDoor.x + ((door.GetComponent<Transform>().localScale.y * 2.0f) - (door.GetComponent<Transform>().localScale.y * 0.2f)))
        {
            door.GetComponent<Transform>().Translate(0, -3f * Time.deltaTime, 0);
        }
    }


////////////////////////////////////////////////////////////////////////
// TRIGGER EXIT ========================================================
    private void OnTriggerExit2D(Collider2D collision)
    {
        timeToReturn = true;
        gemSR.color = ogColor;
        gemLight.SetActive(false);
        doorProfile.DisableGem(ogColor);
    }
}
