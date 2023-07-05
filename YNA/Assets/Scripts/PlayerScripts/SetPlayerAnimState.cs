//*************************************************
// Project: We're Tethered Together
// File: PlayerController.cs
// Author/s: Cameron Myers
//
// Desc: Manage player actions
//
// Notes:
//
// Last Edit: 6/17/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetPlayerAnimState : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    PlayerController pc;

    public enum PlayerStates
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

    private PlayerStates state_curr;
    private PlayerStates state_next;

    [SerializeField]
    private UnityEvent player_idle;
    [SerializeField]
    private UnityEvent player_walk;
    [SerializeField]
    private UnityEvent player_jump;
    [SerializeField]
    private UnityEvent player_falling;
    [SerializeField]
    private UnityEvent player_hide;
    [SerializeField]
    private UnityEvent player_seen;
    [SerializeField]
    private UnityEvent player_dead;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    ////////////////////////////////////////////////////////////////////////
    // FIXED UPDATE ========================================================
    void FixedUpdate()
    {
        check_velocity_y();

        state_curr = state_next;
        
        //player state machine
        switch (state_curr)
        {
            // The player is idle
            case PlayerStates.cIdle:
                {
                    do_idle();
                    break;
                }
            //the player is walking
            case PlayerStates.cWalk:
                {
                    do_walk();
                    break;
                }
            //the player is in an upward motion
            case PlayerStates.cJump:
                {
                    do_jump();
                    break;
                }
            //the player is moving downward
            case PlayerStates.cFall:
                {
                    do_fall();
                    break;
                }
            //the player is hiding behind an object
            case PlayerStates.cHiding:
                {
                    do_hide();
                    break;
                }
            //the player has been spotted
            case PlayerStates.cSeen:
                {
                    do_seen();
                    break;
                }
            //the player is dead
            case PlayerStates.cDead:
                {
                    do_dead();
                    break;
                }
        }

        CheckHide();
    }

    ////////////////////////////////////////////////////////////////////////
    // SET NEXT STATE ======================================================
    public void SetNextState(PlayerStates newState)
    {
        state_next = newState;
    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK HIDING ========================================================
    void CheckHide()
    {
        if (gameObject.GetComponent<Hide>().isHidden == true)
        {
            state_next = PlayerStates.cHiding;
        }
    }


    //***********************************************************************************
    // STATE MACHINE ACTIONS ============================================================
    //***********************************************************************************

    ////////////////////////////////////////////////////////////////////////
    // IS IDLING ===========================================================
    private void do_idle()
    {
        player_idle.Invoke();
    }

    ////////////////////////////////////////////////////////////////////////
    // IS WALKING ==========================================================
    private void do_walk()
    {
        //set walking animation
        player_walk.Invoke();
    }

    ////////////////////////////////////////////////////////////////////////
    // IS JUMPING ==========================================================
    private void do_jump()
    {
        //checks for vertical movement
        player_jump.Invoke();
    }

    ////////////////////////////////////////////////////////////////////////
    // IS FALLING ==========================================================
    private void do_fall()
    {
        if (!pc.IsGrounded())
        {
            //set animation state to falling
            player_falling.Invoke();

        }
        else
        {

            state_next = PlayerStates.cIdle;
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // IS HIDING ===========================================================
    private void do_hide()
    {
        player_hide.Invoke();
    }

    ////////////////////////////////////////////////////////////////////////
    // IS SEEN =============================================================
    private void do_seen()
    {
        player_seen.Invoke();
    }

    ////////////////////////////////////////////////////////////////////////
    // IS DEAD =============================================================
    private void do_dead()
    {
        player_dead.Invoke();
    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK VEL ===========================================================
    // Checks if the player is moving up or down for state machine
    private bool check_velocity_y()
    {
        //check for falling
        if (pc.rb.velocity.y <= -0.02f && !pc.IsGrounded())
        {
            state_next = PlayerStates.cFall;
            return true;
        }
        //check jumping
        else if (pc.rb.velocity.y >= 0.02f)
        {
            state_next = PlayerStates.cJump;
            return true;
        }

        return false;
    }
}
