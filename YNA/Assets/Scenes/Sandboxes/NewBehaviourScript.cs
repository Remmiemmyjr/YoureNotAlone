using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBehaviourScript : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    Rigidbody2D rb;

    [HideInInspector]
    public  Vector2 dir;

    public Transform groundObject;
    public LayerMask layer;

    public float speed = 7f;
    public float jumpHeight = 16f;
    public float smoothDamp = 5f;
    public float smoothRange = 0.05f;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    ////////////////////////////////////////////////////////////////////////
    // FIXED UPDATE ========================================================
    void FixedUpdate()
    {
        if (Mathf.Abs(dir.x) > 0.65)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        }
        else
        {
            rb.AddForce(new Vector2(-(rb.velocity.x * smoothDamp), 0));

            if(Mathf.Abs(rb.velocity.x) <= smoothRange )
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
        Debug.Log(dir.x);
    }


    ////////////////////////////////////////////////////////////////////////
    // JUMP ================================================================
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && IsGrounded())
        {
            rb.velocity = Vector2.up * jumpHeight;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // IS GROUNDED =========================================================
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundObject.position, 0.2f, layer);
    }
}
