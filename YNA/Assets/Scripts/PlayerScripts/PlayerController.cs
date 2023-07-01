//*************************************************
// Project: We're Tethered Together
// File: PlayerController.cs
// Author/s: Emmy Berg
//           Cameron Myers
//
// Desc: Manage player actions
//
// Notes:
//
// Last Edit: 6/23/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor;
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

    private bool isPaused = false;
    GameObject pauseUI;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        pauseUI = GameObject.FindWithTag("Pause");

        if (pauseUI)
        {
            // Hide Pause UI
            pauseUI.SetActive(false);
        }

        animState = GetComponent<SetPlayerAnimState>();
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

        if (dir.x == 0 || !IsGrounded())
        {
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
        if(ctx.performed && cTimeCounter > 0 && !isPaused)
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
    // RESET =============================================================
    public void Reset(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            GetComponent<Stats>().isDead = true;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // PAUSE =============================================================
    public void Pause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // Paused -> Unpaused
            if(isPaused)
            {
                isPaused = false;

                if (pauseUI)
                {
                    // Hide Pause UI
                    pauseUI.SetActive(false);
                }

                // Reset timescale
                Time.timeScale = 1;
            }
            // Unpaused -> Paused
            else
            {
                isPaused = true;

                if (pauseUI)
                {
                    // Show Pause UI
                    pauseUI.SetActive(true);

                    // Set first button as being active
                    GameObject resumeButton = pauseUI.transform.Find("Resume").gameObject;

                    if(resumeButton)
                    {
                        EventSystem.current.SetSelectedGameObject(resumeButton);
                    }
                }

                // Set paused timescale
                Time.timeScale = 0;
            }
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            isPaused = false;
        }
        else
        {
            isPaused = true;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // IS GROUNDED =========================================================
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundObject.position, 0.2f, layer);
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
