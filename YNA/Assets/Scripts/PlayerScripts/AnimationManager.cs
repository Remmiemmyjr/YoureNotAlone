//*************************************************
// Project: We're Tethered Together
// File: AnimationManager.cs
// Author/s: Cameron Myers
//
// Desc: State machine for actors animations
//
// Last Edit: 5/4/2023
//
//*************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

public class AnimationManager : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public enum Character
    {
        Player, Partner
    }

    private Character currChar;

    private Animator animator;


    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        animator = GetComponent<Animator>();

        if(gameObject.tag == "Player")
        {
            currChar = Character.Player;
        }
        else if(gameObject.tag == "Partner")
        {
            currChar = Character.Partner;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {

    }


    ////////////////////////////////////////////////////////////////////////
    // CHECK FLIP ==========================================================
    void SetFlip()
    {
        float velX = gameObject.GetComponent<Rigidbody2D>().velocity.x;

        //check if moving left
        if (velX >= 0.05f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        //check if moving right
        if (velX <= -0.05f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    

    }

    ////////////////////////////////////////////////////////////////////////
    // SET STATE WALK ==========================================================
    public void SetStateWalk()
    {
        //flip to face the correct direction
        SetFlip();
        animator.Play(currChar + "_Walk");

    }

    ////////////////////////////////////////////////////////////////////////
    // SET STATE JUMP ==========================================================
    public void SetStateJump()
    {
        animator.Play(currChar + "_Jump");
    }


    ////////////////////////////////////////////////////////////////////////
    // SET STATE FALL ==========================================================
    public void SetStateFall()
    {
        animator.Play(currChar + "_Fall");
    }


    ////////////////////////////////////////////////////////////////////////
    // CHECK IDLE ==========================================================
    public void SetStateIdle()
    {
        animator.Play(currChar + "_Idle");
    }


    ////////////////////////////////////////////////////////////////////////
    // SET STATE HIDE ======================================================
    public void SetStateHide()
    {
        animator.Play(currChar + "_Hide");

    }


    ////////////////////////////////////////////////////////////////////////
    // SET STATE SEEN ======================================================
    public void SetStateSeen()
    {
        animator.Play(currChar + "_Seen");

    }

    ////////////////////////////////////////////////////////////////////////
    // SET STATE DEAD =======================================================
    public void SetStateDead()
    {
        animator.Play(currChar + "_Dead");

    }


}
