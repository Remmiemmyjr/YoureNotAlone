//*************************************************
// Project: We're Tethered Together
// File: PlayerController.cs
// Author/s: Emmy Berg
//           Cameron Myers
//
// Desc: Manage player actions
//
// Notes:
//  + Fix constant jump timer decreasing bug
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    Rigidbody2D rb;
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

    [HideInInspector]
    public Vector2 dir;

    public Transform groundObject;
    public LayerMask layer;

    public float speed = 7f;
    public float jumpHeight = 16f;
    public float smoothDamp = 5f;
    public float smoothRange = 0.05f;

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
    // START ===============================================================
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    ////////////////////////////////////////////////////////////////////////
    // FIXED UPDATE ========================================================
    void FixedUpdate()
    {
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

        //movement stuff
        if (Mathf.Abs(dir.x) > 0.65)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        }
        else
        {
            rb.AddForce(new Vector2(-(rb.velocity.x * smoothDamp), 0));

            if (Mathf.Abs(rb.velocity.x) <= smoothRange)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // MOVEMENT ============================================================
    public void Movement(InputAction.CallbackContext ctx)
    {
        dir.x = ctx.ReadValue<float>();
        Debug.Log(dir.x);
        //set the state machine to walk
        state_next = PlayerStates.cWalk;

    }


    ////////////////////////////////////////////////////////////////////////
    // JUMP ================================================================
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && IsGrounded())
        {
            rb.velocity = Vector2.up * jumpHeight;

            //set the state machine to jump
            state_next = PlayerStates.cJump;
        }


    }


    ////////////////////////////////////////////////////////////////////////
    // IS GROUNDED =========================================================
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundObject.position, 0.2f, layer);
    }

    ////////////////////////////////////////////////////////////////////////
    // STATE MACHINE ACTIONS ============================================================


    ////////////////////////////////////////////////////////////////////////
    // IS IDLING =========================================================
    private void do_idle()
    {
        //will take care of state change
        //also checks for vertical movement
        if(check_velocity_y())
        {
            return;
        }
        player_idle.Invoke();
    }

    ////////////////////////////////////////////////////////////////////////
    // IS WALKING =========================================================
    private void do_walk()
    {
        //if not walking anymore
        if (dir.x == 0.0f)
        {
            state_next = PlayerStates.cIdle;
            return;
        }
        //will take care of state change
        else if(check_velocity_y())
        {
            return;
        }
        //set walking animation
        player_walk.Invoke();

    }

    ////////////////////////////////////////////////////////////////////////
    // IS JUMPING =========================================================
    private void do_jump()
    {
        //checks for vertical movement
        check_velocity_y();
        player_jump.Invoke();

    }

    ////////////////////////////////////////////////////////////////////////
    // IS FALLING =========================================================
    private void do_fall()
    {
        if (!IsGrounded())
        {
            //set animation state to falling
            player_falling.Invoke();

        }
        else
        {
            state_next = PlayerStates.cWalk;
        }

    }

    ////////////////////////////////////////////////////////////////////////
    // IS HIDING =========================================================
    private void do_hide()
    {
        player_hide.Invoke();

    }

    ////////////////////////////////////////////////////////////////////////
    // IS SEEN =========================================================
    private void do_seen()
    {
        player_seen.Invoke();
    }

    ////////////////////////////////////////////////////////////////////////
    // IS DEAD =========================================================
    private void do_dead()
    {
        player_dead.Invoke();

    }

    ////////////////////////////////////////////////////////////////////////
    // IS DEAD =========================================================
    /// <summary>
    /// Checks if the player is moving up or down for state machine
    /// </summary>
    private bool check_velocity_y()
    {
        //check for falling
        if(rb.velocity.y <= -0.02f)
        {
            state_next = PlayerStates.cFall;
            return true;
        }
        //check jumping
        else if(rb.velocity.y >= 0.02f)
        {
            state_next = PlayerStates.cJump;
            return true;
        }

        return false;
    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK HIDING =========================================================
    void CheckHide()
    {
        if(gameObject.GetComponent<Stats>().isHidden == true)
        {
            state_next = PlayerStates.cHiding;
        }

    }

}
