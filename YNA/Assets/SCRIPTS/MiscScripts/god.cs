using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class god : MonoBehaviour
{
    private ParticleSystem ps;
    private SpriteRenderer spr;

    bool goodbye = true;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Partner")
        {
            if (goodbye)
            {
                goodbye = false;
                ps.Play();
            }

            // Steam Achievement
            spr.enabled = false;
        }
    }
}
