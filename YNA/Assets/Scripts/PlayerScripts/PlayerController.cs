//*************************************************
// Project: We're Tethered Together
// File: PlayerController.cs
// Author/s: Emmy Berg
//           Cameron Myers
//
// Desc: Manage player actions
//
// Notes:
// -
//
// Last Edit: 6/23/2023
//
//*************************************************

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    [HideInInspector]
    public Rigidbody2D rb;
    public ParticleSystem dustParticles;

    GameObject manager;

    private SetPlayerAnimState animState;

    [HideInInspector]
    public Vector2 dir;

    public Transform groundObject;
    public LayerMask layer;

    public float speed = 7f;
    public float jumpHeight = 16f;
    public float smoothDamp = 5f;
    public float smoothRange = 0.05f;

    private float cTime = 0.2f;
    private float cTimeCounter;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        animState = GetComponent<SetPlayerAnimState>();

        manager = GameObject.FindGameObjectWithTag("GameManager");
    }


    ////////////////////////////////////////////////////////////////////////
    // FIXED UPDATE ========================================================
    void FixedUpdate()
    {
        // slow-stop
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

        if (IsStopped() && IsGrounded() && !(gameObject.GetComponent<Hide>().isHidden))
        {
            animState.SetNextState(SetPlayerAnimState.PlayerStates.cIdle);
            StopDustParticles();
        }

        else if (!IsStopped() && IsGrounded() && !(gameObject.GetComponent<Hide>().isHidden))
        {
            animState.SetNextState(SetPlayerAnimState.PlayerStates.cWalk);

            EmitParticles(dir);
        }

        if (!IsGrounded())
        {
            animState.SetNextState(SetPlayerAnimState.PlayerStates.cFall);
            StopDustParticles();
        }

        CoyoteTime();
    }


    ////////////////////////////////////////////////////////////////////////
    // MOVEMENT ============================================================
    public void Movement(InputAction.CallbackContext ctx)
    {
        dir.x = ctx.ReadValue<float>();
       
        animState.SetNextState(SetPlayerAnimState.PlayerStates.cWalk);

        EmitParticles(dir);
    }


    ////////////////////////////////////////////////////////////////////////
    // JUMP ================================================================
    public void Jump(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && cTimeCounter > 0 && !manager.GetComponent<Stats>().isPaused)
        {
            rb.velocity = Vector2.up * jumpHeight;

            animState.SetNextState(SetPlayerAnimState.PlayerStates.cJump);
        }

        if(ctx.canceled && rb.velocity.y > 0)
        {
            cTimeCounter = 0;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // COYOTE TIME =========================================================
    void CoyoteTime()
    {
        if(IsGrounded())
        {
            cTimeCounter = cTime;
        }
        else
        {
            cTimeCounter -= Time.deltaTime;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // IS GROUNDED =========================================================
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundObject.position, 0.2f, layer) || rb.velocity.y == 0;
    }


    ////////////////////////////////////////////////////////////////////////
    // IS STOPPED =========================================================
    public bool IsStopped()
    {
        return Math.Abs(dir.x) < 0.1f;
    }


    ////////////////////////////////////////////////////////////////////////
    // PARTICLES ===========================================================
    void EmitParticles(Vector2 vec)
    {
        if (IsGrounded() && Mathf.Abs(vec.x) > 0)
            CreateDustParticles();
        else
            StopDustParticles();
    }

    void CreateDustParticles()
    {
        dustParticles.Play();
    }
    void StopDustParticles()
    {
        dustParticles.Stop();
    }

}
