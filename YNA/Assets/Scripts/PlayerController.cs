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

public class PlayerController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    private GameObject Player;
    private GameObject Partner;

    Rigidbody2D rb;

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
    
    [HideInInspector]
    public bool onGround;
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
        PlatformerMovement();
    }


    ////////////////////////////////////////////////////////////////////////
    // PLATFORMER MOVEMENT =================================================
    void PlatformerMovement()
    {
        // Movement Code ---
        dir.x = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);

        // Jump Code ---
        groundedRemember -= Time.deltaTime;
        onGround = Physics2D.OverlapCircle(groundObject.position, 0.2f, layer);

        bool isSpace   = Input.GetKeyDown(KeyCode.Space);
        bool isUpArrow = Input.GetKeyDown(KeyCode.UpArrow);

        if (( isSpace || isUpArrow) && (onGround || groundedRemember > 0f) && jumpTimer < 0)
        {
            rb.velocity = Vector2.up * jumpHeight;
            jumpTimer = 0.05f;
        }
        if(onGround)
        {
            groundedRemember = 0.3f;
            jumpTimer -= Time.deltaTime;
        }
    }
}
