// Project: We're Tethered Together
// File: DoorCloseTrigger.cs
// Author/s: Emmy Berg
//
// Desc: Close door, deactivate
//
// Notes:
//  - 
//
// Last Edit: 7/2/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloseTrigger : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    Vector2 ogPosDoor;
    PuzzleDoorProfile doorProfile;

    [SerializeField]
    private Color offColor;
    [SerializeField]
    private Color activeColor;

    public GameObject door;
    bool closeTime = false;

    [SerializeField]
    private AudioClip audioDoorClosed;

    private AudioSource doorAS;

    // Start is called before the first frame update
    void Start()
    {
        doorAS = door.GetComponent<AudioSource>();

        ogPosDoor = door.GetComponent<Transform>().position;
        doorProfile = door.GetComponent<PuzzleDoorProfile>();
        door.GetComponent<SpriteRenderer>().color = activeColor;

        doorProfile.DisableGem(offColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (closeTime)
        {
            if (door.GetComponent<Transform>().position.y > ogPosDoor.y)
            {
                door.GetComponent<Transform>().Translate(0, -9f * Time.deltaTime, 0);
            }
        }
        else if(door.GetComponent<Transform>().position.y < ogPosDoor.y + ((door.GetComponent<Transform>().localScale.y * 2.0f) - (door.GetComponent<Transform>().localScale.y * 0.2f)))
        {
            door.GetComponent<Transform>().Translate(0, 3f * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && closeTime == false)
        {
            closeTime = true;

            doorAS.PlayOneShot(audioDoorClosed);

            doorProfile.EnableGem(activeColor);
        }
    }
}
