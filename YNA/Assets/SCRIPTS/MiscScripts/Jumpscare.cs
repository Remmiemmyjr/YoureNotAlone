using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    public Rigidbody2D rb;
    // Start is called before the first frame update
    private void Awake()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hideable")
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
