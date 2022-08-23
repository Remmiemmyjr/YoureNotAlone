using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    Vector2 ogPosDoor;

    Color ogColor;

    bool timeToReturn = false;

    public GameObject door;


    void Start()
    {
        ogColor = gameObject.GetComponent<SpriteRenderer>().color;
        ogPosDoor = door.GetComponent<Transform>().position;
    }

    private void Update()
    {
        if(timeToReturn)
        {
            if(door.GetComponent<Transform>().position.y > ogPosDoor.y)
            {
                door.GetComponent<Transform>().Translate(0, -0.05f, 0);
            }
            else
            {
                timeToReturn = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        timeToReturn = false;
        if (door.GetComponent<Transform>().position.y < ogPosDoor.y + door.GetComponent<Transform>().localScale.y)
        {
            door.GetComponent<Transform>().Translate(0, 0.1f, 0);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        timeToReturn = true;
        gameObject.GetComponent<SpriteRenderer>().color = ogColor;
    }
}
