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

    float velX;
    float velY;

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
        velX = gameObject.GetComponent<Rigidbody2D>().velocity.x;
        velY = gameObject.GetComponent<Rigidbody2D>().velocity.y;
        
        CheckFlip();
        CheckJump();
        CheckFall();
        CheckWalk();
        CheckIdle();

    }


    void CheckFlip()
    {
        if (velX >= 0.2)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (velX <= -0.2)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    

    }

    void CheckWalk()
    {
        if((velX > 0.4 || velX < -0.4) && velY < 0.5 && velY > -0.5)
        {
            anim.Play(currChar + "_Walk");
        }

    }

    void CheckJump()
    {
        if(velY > 0.5)
        {
            anim.Play(currChar + "_Jump");
        }
    }

    void CheckFall()
    {
        if(velY < -0.5  )
        {
            anim.Play(currChar + "_Fall");
        }
    }

    void CheckIdle()
    {
        if(velX == 0 && velY == 0)
        {
            anim.Play(currChar + "_Idle");
        }
    }

}
