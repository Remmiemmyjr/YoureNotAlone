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

    public float speed = 7f;
    public float jumpHeight = 20f;

    Vector2 dir;

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
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        rb.gravityScale = 5;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.up * jumpHeight;
        }
    }
}
