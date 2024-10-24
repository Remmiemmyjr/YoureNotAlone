//*************************************************
// Project: We're Tethered Together
// File: Checkpoint.cs
// Author/s: Cameron Myers
//           Emmy Berg
//           Corbyn LaMar
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

// README ******************************************************************
// Create a sprite/gameobject and add this script, tag it with "Spawn"
// for every checkpoint after, repeat but tag with "Checkpoint"
// NOTE: the checkpoint tag doesnt actually do anything rn
// *************************************************************************

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
    [SerializeField]
    private bool isSpawn = false;

    [SerializeField]
    private AudioClip[] lanternSFX;
    [SerializeField]
    private AudioClip[] checkpointSFX;

    private AudioSource lanternAS;

    private bool sparkPlayed = false;

    // Boolean to keep track of reached or not
    private bool checkReached = false;

    [SerializeField]
    private bool tetherOnceReached = false;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        EmberPlayer = GetComponent<ParticleSystem>();
        CheckpointLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        lanternAS = GetComponent<AudioSource>();

        if (GameObject.FindWithTag("Partner"))
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

    void PlayLanternFX()
    {
        // Play Particles
        EmberPlayer.Play();

        // Play Audio
        if (lanternSFX.Length > 0 && checkpointSFX.Length > 0)
        {
            lanternAS.PlayOneShot(lanternSFX[Random.Range(0, lanternSFX.Length)]);
            lanternAS.PlayOneShot(checkpointSFX[Random.Range(0, checkpointSFX.Length)]);
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
                if (EmberPlayer && CheckpointLight && lanternAS && !checkReached)
                {
                    // Enable the light
                    CheckpointLight.enabled = true;

                    // Update value
                    checkReached = true;

                    // Do effects
                    PlayLanternFX();
                }

                if(tetherOnceReached)
                {
                    CheckpointController.startWithPartner = true;
                    Info.player.GetComponent<Grapple>().startTetheredTogether = true;
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
                    // Enable the light
                    CheckpointLight.enabled = true;

                    // Update value
                    checkReached = true;

                    // Do effects
                    PlayLanternFX();
                }
                else if (EmberPlayer && !sparkPlayed && !isSpawn)
                {
                    // Do effects
                    PlayLanternFX();

                    // Avoid spam
                    sparkPlayed = true;
                }
            }
        }
    }
}
