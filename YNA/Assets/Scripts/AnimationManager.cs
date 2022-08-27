using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public enum Character
    {
        Player, Partner
    }

    public Character currChar;

    Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();

        if(gameObject.tag == "Player")
        {
            currChar = Character.Player;
        }
        else if(gameObject.tag == "Partner")
        {
            currChar = Character.Partner;
        }
    }


    void Update()
    {
        Flip();
    }


    void Flip()
    {
        if (gameObject.GetComponent<Rigidbody2D>().velocity.x > 0.1)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (gameObject.GetComponent<Rigidbody2D>().velocity.x < -0.1)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        switch(currChar)
        {
            case Character.Partner:
                PlayerWalk();
                break;
        }
    }

    void PlayerWalk()
    {
        anim.SetFloat("speedX", Mathf.Abs(PlayerController.dir.x));
    }
}
