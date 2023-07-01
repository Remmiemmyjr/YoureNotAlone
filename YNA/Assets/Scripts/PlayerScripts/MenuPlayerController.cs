//*************************************************
// Project: We're Tethered Together
// File: MenuPlayerController.cs
// Author/s: Corbyn LaMar
//
// Desc: Manage player actions
//
// Notes:
//
// Last Edit: 6/30/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static System.Net.WebRequestMethods;

public class MenuPlayerController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    [HideInInspector]
    public Rigidbody2D rb;

    [HideInInspector]
    public Vector2 dir;

    public float speed = 7f;
    public float smoothDamp = 5f;
    public float smoothRange = 0.05f;

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
    }


    ////////////////////////////////////////////////////////////////////////
    // FIXED UPDATE ========================================================
    void FixedUpdate()
    {
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
    }
}
