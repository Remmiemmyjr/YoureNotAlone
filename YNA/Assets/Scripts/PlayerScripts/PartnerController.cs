using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartnerController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    Rigidbody2D rb;
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
    private PartnerStates state_next;

    [HideInInspector]
    public Vector2 dir;

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
    private UnityEvent partner_dead;
    [SerializeField]
    private UnityEvent partner_seen;

    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    ////////////////////////////////////////////////////////////////////////
    // FIXED UPDATE ========================================================
    void Update()
    {
        //TODO:
        //check for what state to be in


        //set state

        state_curr = state_next;
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




    ////////////////////////////////////////////////////////////////////////
    // IS GROUNDED =========================================================
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundObject.position, 0.2f, layer);
    }

    ////////////////////////////////////////////////////////////////////////
    // IS FALLING =========================================================
    public bool IsFalling()
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
        partner_idle.Invoke();
    }

    ////////////////////////////////////////////////////////////////////////
    // IS WALKING =========================================================
    private void do_walk()
    {
        //if not walking anymore
        if (dir.x == 0.0f)
        {
            state_next = PartnerStates.cIdle;
            return;
        }
        //set walking animation
        partner_walk.Invoke();

        //call wwise walking event


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


        //if just jumped post jump wwise event
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
        else if (dir.x == 0.0f)
        {
            state_next = PartnerStates.cIdle;
        }
        else
        {
            state_next = PartnerStates.cWalk;
        }


    }

    ////////////////////////////////////////////////////////////////////////
    // IS HIDING =========================================================
    private void do_hide()
    {
        partner_hide.Invoke();

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
