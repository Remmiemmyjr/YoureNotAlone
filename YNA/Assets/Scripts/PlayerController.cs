using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    [HideInInspector]
    public static Vector2 dir;

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

    private CheckpointController cc;
    private GameObject Player;
    private GameObject Partner;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //get the CC to get access to the last checkpoint
        cc = GameObject.FindGameObjectWithTag("CC").GetComponent<CheckpointController>();
        //get the players game objects
        Player = GameObject.FindGameObjectWithTag("Player");
        Partner = GameObject.FindGameObjectWithTag("Partner");

        //set pos to last checkpoint
        Player.transform.position = cc.lastCheckpointPos;
        Partner.transform.position = cc.lastCheckpointPos;
    }



    void Update()
    {
        PlatformerMovement();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }



    void PlatformerMovement()
    {
        // Movement Code
        dir.x = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);

        Debug.Log(rb.velocity);


        // Jump Code
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

        //Debug.Log("**************** Jump *****************");
        //if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && (onGround || groundedRemember > 0f) && jumpTimer < 0)
        //if (isSpace || isUpArrow)
        //    Debug.Log($"Space:{isSpace}, UpArrow:{isUpArrow}, Ground:{onGround}, Remember:{groundedRemember}, JumpTimer:{jumpTimer}, Velocity:{rb.velocity}");
    }



    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(groundObject.position, 0.2f);
    //}
}
