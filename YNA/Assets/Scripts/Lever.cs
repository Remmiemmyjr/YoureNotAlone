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

    public UnityEngine.Rendering.Universal.Light2D torch;
    public GameObject eyeManager;
    public float returnTime = 5f;
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
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                //turn eyes back on
                torch.intensity = 1;
                eyeManager.GetComponent<ActivateEyes>().canActivate = true;

            }
        }
    }

    private void OnTriggerEnter2D()
    {
        //check for action key press

            //set lever state
            leverState = true;

            gameObject.GetComponent<SpriteRenderer>().flipX = true;

            //gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            
            //set eyes to off
            torch.intensity = 0;
            eyeManager.GetComponent<ActivateEyes>().canActivate = false;
    }
}
