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

    private PlayerStates state;

    [HideInInspector]
    public Vector2 dir;

    public Transform groundObject;
    public LayerMask layer;

    public float speed = 7f;
    public float jumpHeight = 16f;
    public float smoothDamp = 5f;
    public float smoothRange = 0.05f;


    [SerializeField]
    public UnityEvent player_idle;
    [SerializeField]  
    public UnityEvent player_walk;
    [SerializeField]  
    public UnityEvent player_jump;
    [SerializeField]  
    public UnityEvent player_falling;
    [SerializeField]  
    public UnityEvent player_hide;
    [SerializeField]  
    public UnityEvent player_dead;
    [SerializeField]  
    public UnityEvent player_seen;

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

        //player state machine

        switch (state)
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
        state = PlayerStates.cWalk;

    }


    ////////////////////////////////////////////////////////////////////////
    // JUMP ================================================================
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && IsGrounded())
        {
            rb.velocity = Vector2.up * jumpHeight;

            //set the state machine to jump
            state = PlayerStates.cJump;
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
        //set animation state to idle
        player_idle.Invoke();
    }

    ////////////////////////////////////////////////////////////////////////
    // IS WALKING =========================================================
    private void do_walk()
    {
        //if not walking anymore
        if (dir.x == 0.0f)
        {
            state = PlayerStates.cIdle;
            return;
        }
        //set walking animation
        player_walk.Invoke();

        //call wwise walking event
     

    }

    ////////////////////////////////////////////////////////////////////////
    // IS JUMPING =========================================================
    private void do_jump()
    {
        //set jump animation based on y velocity
        if (rb.velocity.y >= 0.0f)
        {
            player_jump.Invoke();

        }
        //if y velocity is downward set state to falling and not grounded
        else
        {
            state = PlayerStates.cFall;
        }


        //if just jumped post jump wwise event
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
            state = PlayerStates.cIdle;
        }


    }

    ////////////////////////////////////////////////////////////////////////
    // IS HIDING =========================================================
    private void do_hide()
    {
        player_hide.Invoke();

        //set animation state to hiding
    }

    ////////////////////////////////////////////////////////////////////////
    // IS SEEN =========================================================
    private void do_seen()
    {
        player_seen.Invoke();

        //set animation state to seen
    }

    ////////////////////////////////////////////////////////////////////////
    // IS DEAD =========================================================
    private void do_dead()
    {

        //set dead animation state
        player_dead.Invoke();

        //trigger game over events
    }
}



// old jump code
//if ((isSpace || isUpArrow) && (onGround || groundedRemember > 0f) && jumpTimer < 0)
//{
//    rb.velocity = Vector2.up * jumpHeight;
//    jumpTimer = 0.05f;
//}
//if (onGround)
//{
//    groundedRemember = 0.3f;
//    jumpTimer -= Time.deltaTime;
//}
