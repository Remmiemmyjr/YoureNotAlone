//*************************************************
// Project: We're Tethered Together
// File: Parallax.cs
// Author/s: Cameron Myers
//
// Desc: Background parallaxing effect
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    private float length, startpos;
    private float camPos;
    GameObject cam;
    public float parallaxEffect;
    // *********************************************************************

    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        cam = GameObject.FindWithTag("MainCamera");
    }


        ////////////////////////////////////////////////////////////////////////
        // START ===============================================================
        void Start()
    {
        camPos = cam.transform.position.x;
        
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float deltaDist = (cam.transform.position.x - camPos) * parallaxEffect;
        

        startpos = transform.position.x;
        float camDist = (cam.transform.position.x - transform.position.x);


        if (camDist > length)
        {
            startpos += length;
        }

        else if (camDist < -length)
        {
            startpos -= length;
        }

        camPos = cam.transform.position.x;

        transform.position = new Vector3(startpos + deltaDist, transform.position.y, transform.position.z);
    }
}
