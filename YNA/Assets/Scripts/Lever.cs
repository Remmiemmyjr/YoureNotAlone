//*************************************************
// Project: We're Tethered Together
// File: MenuManager.cs
// Author/s: Emmy Berg
//
// Desc: Lever controller for torch extinguish
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//HOW TO ADD LEVER TO SCENE
//create 2d object, add polygon collider, turn on isTrigger
//add lever script
//assign the main camera to the script(its kinda hacky but it works for now)
public class Lever : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public GameObject eyeManager;

    public UnityEngine.Rendering.Universal.Light2D torch;

    bool leverState = false;

    [SerializeField] float returnTime = 5f;
    float startTime = 0f;

    // optional test variable
    Color ogColor;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        ogColor = gameObject.GetComponent<SpriteRenderer>().color;
        
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
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


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D()
    {
        //set lever state
        leverState = true;

        gameObject.GetComponent<SpriteRenderer>().flipX = true;
        
        //set eyes to off
        torch.intensity = 0;
        eyeManager.GetComponent<ActivateEyes>().canActivate = false;
    }
}
