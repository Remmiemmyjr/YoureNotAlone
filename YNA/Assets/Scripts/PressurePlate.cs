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

    [SerializeField]
    private bool isHorizontal = false;


    void Start()
    {
        ogColor = gameObject.GetComponent<SpriteRenderer>().color;
        ogPosDoor = door.GetComponent<Transform>().position;
    }


    private void Update()
    {
        if(timeToReturn)
        {
            if(!isHorizontal && door.GetComponent<Transform>().position.y > ogPosDoor.y)
            {
                door.GetComponent<Transform>().Translate(0, -7f * Time.deltaTime, 0);
            }
            else if(isHorizontal && door.GetComponent<Transform>().position.x > ogPosDoor.x)
            {
                door.GetComponent<Transform>().Translate(0, 7f * Time.deltaTime, 0);
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
        if (!isHorizontal && door.GetComponent<Transform>().position.y < ogPosDoor.y + ((door.GetComponent<Transform>().localScale.y * 2.0f) - (door.GetComponent<Transform>().localScale.y * 0.2f)))
        {
            door.GetComponent<Transform>().Translate(0, 7f * Time.deltaTime, 0);
        }
        else if (isHorizontal && door.GetComponent<Transform>().position.x < ogPosDoor.x + ((door.GetComponent<Transform>().localScale.y * 2.0f) - (door.GetComponent<Transform>().localScale.y * 0.2f)))
        {
            door.GetComponent<Transform>().Translate(0, -7f * Time.deltaTime, 0);
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
