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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public enum Character
    {
        Player, Partner
    }

    public Character currChar;

    Animator anim;

    float velX;
    float velY;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        anim = GetComponent<Animator>();

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
        
        CheckFlip();
        CheckJump();
        CheckFall();
        CheckWalk();
        CheckIdle();
    }


    ////////////////////////////////////////////////////////////////////////
    // CHECK FLIP ==========================================================
    void CheckFlip()
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
    void CheckWalk()
    {
        if((velX > 1 || velX < -1) && velY < 1 && velY > -1)
        {
            anim.Play(currChar + "_Walk");
        }

    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK JUMP ==========================================================
    void CheckJump()
    {
        if(velY > 1 && velY != 0)
        {
            anim.Play(currChar + "_Jump");
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // CHECK FALL ==========================================================
    void CheckFall()
    {
        if(velY < -1 && velY != 0)
        {
            anim.Play(currChar + "_Fall");
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // CHECK IDLE ==========================================================
    void CheckIdle()
    {
        if(velX == 0 && velY == 0)
        {
            anim.Play(currChar + "_Idle");
        }
    }

}
