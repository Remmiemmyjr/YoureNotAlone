using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 dir;

    public Transform groundObject;
    public LayerMask layer;

    public float speed = 7f;
    public float jumpHeight = 16.5f;
    
    [HideInInspector]
    public bool onGround; 


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        // Jump Code
        onGround = Physics2D.OverlapCircle(groundObject.position, 0.2f, layer);

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && onGround)
        {
            rb.velocity = Vector2.up * jumpHeight;
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundObject.position, 0.2f);
    }
}
