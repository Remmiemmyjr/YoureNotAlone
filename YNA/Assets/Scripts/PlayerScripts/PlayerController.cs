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
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    private GameObject Player;
    private GameObject Partner;

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
    public static Vector2 dir;
    [HideInInspector]
    public Vector2 partnerOffset;

    public Transform groundObject;
    public LayerMask layer;

    public float speed = 7f;
    public float jumpHeight = 16f;
    [SerializeField]
    public float groundedRemember = 0.0f;
    [SerializeField]
    public float jumpTimer = 0.05f;

    // move to scenemanager or something
    [HideInInspector]
    public bool dontSpawnPartner;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Player = GameObject.FindGameObjectWithTag("Player");
        Partner = GameObject.FindGameObjectWithTag("Partner");

        partnerOffset = new Vector2(-1, 0);

        //set pos to last checkpoint
        Player.transform.position = CheckpointController.lastCheckpointPos;
        if (!dontSpawnPartner)
        {
            Partner.transform.position = CheckpointController.lastCheckpointPos - partnerOffset;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
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



        // constantly update the velocities movement
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }


    ////////////////////////////////////////////////////////////////////////
    // MOVEMENT ============================================================
    public void Movement(InputAction.CallbackContext ctx)
    {
        dir.x = ctx.ReadValue<float>();
    }


    ////////////////////////////////////////////////////////////////////////
    // JUMP ================================================================
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && IsGrounded())
        {
            rb.velocity = Vector2.up * jumpHeight;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // IS GROUNDED =========================================================
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundObject.position, 0.2f, layer);
    }



    ////////////////////////////////////////////////////////////////////////
    // ACTION FUNCTIONS ============================================================


    ////////////////////////////////////////////////////////////////////////
    // IS IDLING =========================================================
    private void do_idle()
    {

    }

    ////////////////////////////////////////////////////////////////////////
    // IS WALKING =========================================================
    private void do_walk()
    {

    }

    ////////////////////////////////////////////////////////////////////////
    // IS JUMPING =========================================================
    private void do_jump()
    {

    }

    ////////////////////////////////////////////////////////////////////////
    // IS FALLING =========================================================
    private void do_fall()
    {

    }

    ////////////////////////////////////////////////////////////////////////
    // IS HIDING =========================================================
    private void do_hide()
    {

    }

    ////////////////////////////////////////////////////////////////////////
    // IS SEEN =========================================================
    private void do_seen()
    {

    }

    ////////////////////////////////////////////////////////////////////////
    // IS DEAD =========================================================
    private void do_dead()
    {

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
