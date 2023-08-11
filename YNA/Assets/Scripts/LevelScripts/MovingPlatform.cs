using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement;

    public GameObject leftBlock;
    public GameObject rightBlock;
    public float speed = 3f;

    [SerializeField]
    private bool isHorizontal = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(isHorizontal)
        {
            movement = new Vector2(1, 0);
            //rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            movement = new Vector2(0, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movement * (speed * Time.deltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PlatformBarrier")
        {
            speed *= -1;
        }
    }
}
