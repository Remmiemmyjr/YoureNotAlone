using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    Vector2 ogPosDoor;

    Color ogColor;

    bool timeToReturn = false;
    bool onPlate;

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
                door.GetComponent<Transform>().Translate(0, -7f * Time.deltaTime, 0);
            }
            else
            {
                timeToReturn = false;
            }
        }

        if(onPlate)
        {

        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onPlate = true;
    }


    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.35f, 0.51f, 0.39f);

        timeToReturn = false;
        if (door.GetComponent<Transform>().position.y < ogPosDoor.y + door.GetComponent<Transform>().localScale.y + 2)
        {
            door.GetComponent<Transform>().Translate(0, 7f * Time.deltaTime, 0);
        }

    }


    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onPlate = false;
        timeToReturn = true;
        gameObject.GetComponent<SpriteRenderer>().color = ogColor;
    }
}
