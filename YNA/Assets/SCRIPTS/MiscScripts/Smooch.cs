// Project: We're Tethered Together
// File: Smooch.cs
// Author/s: Emmy Berg
//
// Desc: Smooch particles and SFX on "tether" in L6
//
// Notes:
//  - .3.
//
// Last Edit: 2/14/2024
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Smooch : MonoBehaviour
{
    float smoochLength;
    float maxSmoochTime = 0.5f;
    public ParticleSystem smooches;
    public AudioClip[] randClip;
    public AudioSource source;

    bool canSmooch = false;
    bool smoochReady = true;

    void Awake()
    {
        canSmooch = false;
        smoochLength = maxSmoochTime;
    }

    void Update()
    {
        if (smoochLength > 0)
        {
            smoochLength -= Time.deltaTime;
            Info.player.GetComponent<Animator>().Play("Player_Smooch");
            Info.player.GetComponent<PlayerController>().isSmooching = true;
        }

        if (smoochLength <= 0)
        {
            Info.player.GetComponent<PlayerController>().isSmooching = false;
            //Info.player.GetComponent<PlayerController>().animState.SetNextState(SetPlayerAnimState.PlayerStates.cIdle);
            smoochReady = true;
        }
    }


    public void GiveASmooch(InputAction.CallbackContext ctx)
    {
        if (canSmooch && smoochReady)
        {
            smoochLength = maxSmoochTime;
            smoochReady = false;
            Info.player.GetComponent<PlayerController>().isSmooching = true;

            smooches.Play();

            source.PlayOneShot(randClip[Random.Range(0, randClip.Length)]);
            Info.player.GetComponent<Animator>().Play("Player_Smooch");

            GetComponent<SteamForceAwardAchievement>().AwardAchievement();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.tag == "Statue")
        {
            canSmooch = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.tag == "Statue")
        {
            canSmooch = false;
        }
    }
}
