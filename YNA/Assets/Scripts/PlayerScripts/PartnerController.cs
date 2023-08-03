//*************************************************
// Project: We're Tethered Together
// File: PartnerController.cs
// Author/s: Emmy Berg
//           Cameron Myers
//
// Desc: Manage partner actions & animation
//
// Notes:
// -
//
// Last Edit: 7/16/2023
//
//*************************************************

using System;
using UnityEngine;
using UnityEngine.Events;

public class PartnerController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    Rigidbody2D rb;
    Stats manager;

    public enum PartnerStates
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

    private PartnerStates state_curr;
    [HideInInspector]
    public PartnerStates state_next;

    public Transform groundObject;
    public LayerMask layer;

    [SerializeField]
    private UnityEvent partner_idle;
    [SerializeField]
    private UnityEvent partner_walk;
    [SerializeField]
    private UnityEvent partner_jump;
    [SerializeField]
    private UnityEvent partner_falling;
    [SerializeField]
    private UnityEvent partner_hide;
    [SerializeField]
    private UnityEvent partner_seen;
    [SerializeField]
    private UnityEvent partner_dead;


    //partner data
    private Vector2 velocity;

    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Stats>();
    }



    ////////////////////////////////////////////////////////////////////////
    // FIXED UPDATE ========================================================
    void Update()
    {
        if (Info.isDead) return;

        velocity = rb.velocity;        

        //TODO:
        //check for what state to be in
        CheckFlip();
        CheckWalk();
        CheckJump();
        CheckFall();
        CheckHide();
        CheckSeen();
        CheckIdle();

        //set state
        state_curr = state_next;

        if (!Info.isDead)
        {
            //partner state machine
            //do actions
            switch (state_curr)
            {
                // The partner is idle
                case PartnerStates.cIdle:
                    {
                        do_idle();
                        break;
                    }
                //the partner is walking
                case PartnerStates.cWalk:
                    {
                        do_walk();
                        break;
                    }
                //the partner is in an upward motion
                case PartnerStates.cJump:
                    {
                        do_jump();
                        break;
                    }
                //the partner is moving downward
                case PartnerStates.cFall:
                    {
                        do_fall();
                        break;
                    }
                //the partner is hiding behind an object
                case PartnerStates.cHiding:
                    {
                        do_hide();
                        break;
                    }
                //the partner has been spotted
                case PartnerStates.cSeen:
                    {
                        do_seen();
                        break;
                    }
                //the partner is dead
                case PartnerStates.cDead:
                    {
                        do_dead();
                        break;
                    }
            }
        }

    }


    ////////////////////////////////////////////////////////////////////////
    // IS GROUNDED =========================================================
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundObject.position, 0.2f, layer);
    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK FLIP ==========================================================
    void CheckFlip()
    {
        if (velocity.x >= 0.05f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (velocity.x <= -0.05f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK WALK ==========================================================
    void CheckWalk()
    {
        if (!IsStopped() && IsGrounded())
        {
            state_next = PartnerStates.cWalk;
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK JUMP ==========================================================
    void CheckJump()
    {
        if (velocity.y > 0.05f && velocity.y != 0.0f)
        {
            state_next = PartnerStates.cJump;

        }
    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK FALL ==========================================================
    void CheckFall()
    {
        if (!IsGrounded() && (velocity.y < -0.05f && velocity.y != 0.0f))
        {
            state_next = PartnerStates.cFall;
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK IDLE ==========================================================
    void CheckIdle()
    {
        if (IsStopped() && gameObject.GetComponent<Hide>().isHidden == false && IsGrounded())
        {
            state_next = PartnerStates.cIdle;
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK HIDING =========================================================
    void CheckHide()
    {
        if(gameObject.GetComponent<Hide>().isHidden && IsStopped())
        {
            state_next = PartnerStates.cHiding;
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // CHECK SEEN =========================================================
    void CheckSeen()
    {
        //change to seen check
        if(gameObject.GetComponent<Hide>().isHidden == true)
        {
            state_next = PartnerStates.cHiding;
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // IS STOPPED =========================================================
    public bool IsStopped()
    {
        return Math.Abs(velocity.x) < 0.05f;
    }

    //***********************************************************************************
    // STATE MACHINE ACTIONS ============================================================
    //***********************************************************************************

    ////////////////////////////////////////////////////////////////////////
    // IS IDLING =========================================================
    private void do_idle()
    {
        //set animation state to idle
        partner_idle.Invoke();
    }

    ////////////////////////////////////////////////////////////////////////
    // IS WALKING =========================================================
    private void do_walk()
    {
        //if not walking anymore
        if (IsStopped())
        {
            state_next = PartnerStates.cIdle;
            return;
        }

        //set walking animation
        partner_walk.Invoke();
    }

    ////////////////////////////////////////////////////////////////////////
    // IS JUMPING =========================================================
    private void do_jump()
    {
        //set jump animation based on y velocity
        if (rb.velocity.y >= 0.0f)
        {
            partner_jump.Invoke();

        }
        //if y velocity is downward set state to falling and not grounded
        else
        {
            state_next = PartnerStates.cFall;
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // IS FALLING =========================================================
    private void do_fall()
    {
        if (!IsGrounded())
        {
            //set animation state to falling
            partner_falling.Invoke();

        }
        //else
        //{
        //    state_next = PartnerStates.cWalk;
        //}
    }

    ////////////////////////////////////////////////////////////////////////
    // IS HIDING =========================================================
    private void do_hide()
    {
        if (IsStopped())
        {
            partner_hide.Invoke();
        }

        state_next = PartnerStates.cWalk;

        //set animation state to hiding
    }

    ////////////////////////////////////////////////////////////////////////
    // IS SEEN =========================================================
    private void do_seen()
    {
        partner_seen.Invoke();

        //set animation state to seen
    }

    ////////////////////////////////////////////////////////////////////////
    // IS DEAD =========================================================
    private void do_dead()
    {
        //set dead animation state
        partner_dead.Invoke();

        //trigger game over events
    }
}
