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

    public enum AnimationStates
    {
        cInvalid = -1,
        cIdle = 0,
        cJump,
        cWalk,
        cFall,
        cHiding,
        cSeen,
        cDead
    }

    
    private Animator animator;

    float velX;
    float velY;




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
        velX = gameObject.GetComponent<Rigidbody2D>().velocity.x;
        velY = gameObject.GetComponent<Rigidbody2D>().velocity.y;

    }


    ////////////////////////////////////////////////////////////////////////
    // CHECK FLIP ==========================================================
    void SetFlip()
    {
        if (velX >= 0.2)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (velX <= -0.2)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    

    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK WALK ==========================================================
    
    public void SetStateWalk()
    {
        SetFlip();
        animator.Play(currChar + "_Walk");

    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK JUMP ==========================================================
    public void SetStateJump()
    {
        animator.Play(currChar + "_Jump");
    }


    ////////////////////////////////////////////////////////////////////////
    // CHECK FALL ==========================================================
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

    }


    ////////////////////////////////////////////////////////////////////////
    // SET STATE SEEN ======================================================
    public void SetStateSeen()
    {

    }

    ////////////////////////////////////////////////////////////////////////
    // SET STATE DEAD =======================================================
    public void SetStateDead()
    {

    }


}
