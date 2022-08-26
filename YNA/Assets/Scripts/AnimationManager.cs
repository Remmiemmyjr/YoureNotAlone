using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public enum Character
    {
        Player, Partner
    }

    Character currChar;


    void Start()
    {
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

        if(currChar == Character.Player)
        {

        }
    }
}
