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
//  -
//
// Last Edit: 7/16/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private AudioClip audioDoorOpened;
    [SerializeField]
    private AudioClip audioDoorClosed;

    private bool playedOpenAudio = false;
    private bool playedClosedAudio = false;

    private AudioSource doorAS;

    //The list of colliders currently inside the trigger
     List<Collider2D> TriggerList = new List<Collider2D>();

    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        doorAS = door.GetComponent<AudioSource>();

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
            if (!playedClosedAudio)
            {
                doorAS.PlayOneShot(audioDoorClosed);
                playedOpenAudio = false;
                playedClosedAudio = true;
            }

            if (!isHorizontal && door.GetComponent<Transform>().position.y > ogPosDoor.y)
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
        // If the object is not already in the list
        if (!TriggerList.Contains(collision))
        {
            // Add the object to the list
            TriggerList.Add(collision);

            if (TriggerList.Count == 1)
            {
                gemSR.color = activeColor;
                gemLight.SetActive(true);
                doorProfile.EnableGem(activeColor);

                if (!playedOpenAudio)
                {
                    doorAS.PlayOneShot(audioDoorOpened);
                    playedClosedAudio = false;
                    playedOpenAudio = true;
                }
            }
        }

        timeToReturn = false;
        if (!isHorizontal && door.GetComponent<Transform>().position.y < ogPosDoor.y + ((door.GetComponent<Transform>().localScale.y * 2.0f) - (door.GetComponent<Transform>().localScale.y * 0.2f)))
        {
            door.GetComponent<Transform>().Translate(0, 3f * Time.deltaTime, 0);
        }
        else if (isHorizontal && door.GetComponent<Transform>().position.x < ogPosDoor.x + ((door.GetComponent<Transform>().localScale.y * 2.0f + 0.25f) - (door.GetComponent<Transform>().localScale.y * 0.2f)))
        {
            door.GetComponent<Transform>().Translate(0, -3f * Time.deltaTime, 0);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER EXIT ========================================================
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If the object is in the list
        if (TriggerList.Contains(collision))
        {
            // Remove it from the list
            TriggerList.Remove(collision);

            if (TriggerList.Count == 0)
            {
                timeToReturn = true;
                gemSR.color = ogColor;
                gemLight.SetActive(false);
                doorProfile.DisableGem(ogColor);
            }
        }
    }
}
