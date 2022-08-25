using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HOW TO ADD LEVER TO SCENE
//create 2d object, add polygon collider, turn on isTrigger
//add lever script
//assign the main camera to the script(its kinda hacky but it works for now)
public class Lever : MonoBehaviour
{


    //var for showing lever state
    bool leverState = false;

    public GameObject cam;
    public float returnTime = 3f;
    float startTime = 0f;
    // vars for testing

    Color ogColor;


    void Start()
    {
        ogColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!leverState) startTime = Time.fixedTime;
        {
            if(Time.fixedTime > startTime + returnTime)
            {
                leverState = false;
                gameObject.GetComponent<SpriteRenderer>().color = ogColor;
                gameObject.GetComponent<SpriteRenderer>().flipY = false;
                //turn eyes back on
                cam.GetComponent<ActivateEyes>().canActivate = true;

            }
        }
    }

    private void OnTriggerEnter2D()
    {
        //check for action key press

            //set lever state
            leverState = true;

            gameObject.GetComponent<SpriteRenderer>().flipY = true;

            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            
            //set eyes to off
            cam.GetComponent<ActivateEyes>().canActivate = false;
    }
}
