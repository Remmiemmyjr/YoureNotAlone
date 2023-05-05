//*************************************************
// Project: We're Tethered Together
// File: PlayerFootstepAudio.cs
// Author/s: K Preston
//
// Desc: Manages player footstep sfx
//
// Notes:
//  + Add footsteps to partner
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepAudio : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public GameObject player;
    public PlayerController _moveScript;
    public AnimationManager _animationManager;
    public PlayAudio randClip;
    public float playInterval;
    private AudioSource source;
    public float resetTime;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        source = GetComponent<AudioSource>();
        resetTime = playInterval;
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        // TODO: If the player is not walking, reset the sound timer. 

        if ((Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.D) |
            Input.GetKey(KeyCode.LeftArrow) | Input.GetKey(KeyCode.RightArrow)) & _moveScript.onGround)
        {
            resetTime -= Time.deltaTime;

            if (resetTime <= 0)
            {
                source.PlayOneShot(randClip.GetRandomClip());
                resetTime = playInterval;
            }
        }
    }
}