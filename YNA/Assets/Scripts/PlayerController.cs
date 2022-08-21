using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum ControllerType
    {
        Topdown,
        Platformer
    }
    public ControllerType ct;

    Rigidbody2D rb;

    Vector2 dir;

    public Transform groundObject;

    public LayerMask layer;

    public float speed = 7f;
    public float jumpHeight = 20f;
    public float numJumps = 1;
    
    bool onGround; 


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ct = ControllerType.Platformer;
    }

    // Update is called once per frame
    void Update()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");

        if (ct == ControllerType.Topdown)
        {
            TopdownMovement();
        }
        else 
        {
            PlatformerMovement();
            Debug.Log(numJumps);
        }
    }

    void TopdownMovement()
    {
        dir.Normalize();
        rb.velocity = new Vector2(dir.x * speed, dir.y * speed);
        rb.gravityScale = 0;
    }

    void PlatformerMovement()
    {
        onGround = Physics2D.OverlapCircle(groundObject.position, 0.1f, layer);

        //if(onGround)
        //{
        //    numJumps = 1;
        //}

        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        rb.gravityScale = 5;

        // Preventing double jumps 
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            //--numJumps;
            rb.velocity = Vector2.up * jumpHeight;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundObject.position, 0.1f);
    }
}
