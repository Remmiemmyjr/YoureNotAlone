using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 dragVel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        dragVel = new Vector2(-rb.velocity.x, 0);
        rb.AddForce(dragVel * 2, ForceMode2D.Force);
    }
}
