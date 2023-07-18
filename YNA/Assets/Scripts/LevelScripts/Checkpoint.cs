//*************************************************
// Project: We're Tethered Together
// File: Checkpoint.cs
// Author/s: Emmy Berg
//           Cameron Myers
//
// Desc: Functionality for checkpoint objects
//
// Notes:
// -
//
// Last Edit: 7/16/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// README ***********************************************************************
// Create a sprite/gameobject and add this script, tag it with "Spawn"
// for every checkpoint after, repeat but tag with "Checkpoint"
// NOTE: the checkpoint tag doesnt actually do anything rn
// ******************************************************************************

public class Checkpoint : MonoBehaviour
{
////////////////////////////////////////////////////////////////////////
// VARIABLES ===========================================================
    //private CheckpointController cc;
    public Sprite CheckOn;

    // Check to use partner for checkpoint
    private bool usePlayer = true;

    // Fancy little effects
    private ParticleSystem EmberPlayer;
    private UnityEngine.Rendering.Universal.Light2D CheckpointLight;

    [SerializeField]
    private float lightGrowRate = 1.0f;

    [SerializeField]
    private bool startLit = false;

    private bool sparkPlayed = false;

    // Boolean to keep track of reached or not
    private bool checkReached = false;
// *********************************************************************


////////////////////////////////////////////////////////////////////////
// START ===============================================================
    void Start()
    {
        EmberPlayer = GetComponent<ParticleSystem>();
        CheckpointLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        if(GameObject.FindWithTag("Partner"))
        {
            usePlayer = false;
        }

        // Activate spawn lights on scene start
        if (startLit)
        {
            GetComponent<SpriteRenderer>().sprite = CheckOn;

            // Make sure extra items exist
            if (EmberPlayer && CheckpointLight && !checkReached)
            {
                // Play Particles
                EmberPlayer.Play();

                // Enable the light
                CheckpointLight.enabled = true;

                // Update value
                checkReached = true;
            }
        }
    }


////////////////////////////////////////////////////////////////////////
// UPDATE ==============================================================
    void Update()
    {
        // If the checkpoint has been activated and the light is less than intended, grow it
        if (checkReached && CheckpointLight.pointLightOuterRadius < 3)
        {
            CheckpointLight.pointLightOuterRadius += lightGrowRate * Time.deltaTime;
        }
    }


////////////////////////////////////////////////////////////////////////
// TRIGGER ENTER =======================================================
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!usePlayer)
        {
            // Check for checkpoint reached by partner
            if (other.CompareTag("Partner"))
            {
                CheckpointController.lastCheckpointPos = transform.position;

                GetComponent<SpriteRenderer>().sprite = CheckOn;

                // Make sure extra items exist
                if (EmberPlayer && CheckpointLight && !checkReached)
                {
                    // Play Particles
                    EmberPlayer.Play();

                    // Enable the light
                    CheckpointLight.enabled = true;

                    // Update value
                    checkReached = true;
                }
            }
        }
        else
        {
            // Check for checkpoint reached by player
            if (other.CompareTag("Player"))
            {
                CheckpointController.lastCheckpointPos = transform.position;

                GetComponent<SpriteRenderer>().sprite = CheckOn;

                // Make sure extra items exist
                if (EmberPlayer && CheckpointLight && !checkReached)
                {
                    // Play Particles
                    EmberPlayer.Play();

                    // Enable the light
                    CheckpointLight.enabled = true;

                    // Update value
                    checkReached = true;
                }
                else if (EmberPlayer && !sparkPlayed)
                {
                    // Play Particles
                    EmberPlayer.Play();

                    // Avoid spam
                    sparkPlayed = true;
                }
            }
        }
    }
}
