using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneProfile : MonoBehaviour
{
    SpriteRenderer spr;
    GameObject owner;
    Animator ownerAnim;
    Stats manager;
    bool shouldDoThis;

    void Awake()
    {
        shouldDoThis = true;
        spr = GetComponent<SpriteRenderer>();
        owner = transform.parent.gameObject;
        ownerAnim = owner.GetComponent<Animator>();
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Stats>();
    }

    void Update()
    {
        // y no workie
        spr.flipX = owner.GetComponent<SpriteRenderer>().flipX;

        if (manager.isDead && shouldDoThis)
        {
            shouldDoThis = false;
            if (owner.tag == "Player")
                ownerAnim.Play("Player_Still");

            else if (owner.tag == "Partner")
                ownerAnim.Play("Partner_Still");
        }
    }
}
