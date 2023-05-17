using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




public class CameraFX : MonoBehaviour
{
    public enum FX
    {
        ChromaticAbberation,
        Vignette,
        Bloom,
        FilmGrain,
    }
    private bool vignette_toggle = false;

    //vignette vars
    float vignette_start_val = 0.0f;
    float vignette_goal_val = 0.0f;

    float vignette_lerp_time = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(vignette_toggle);
    }

    public void test()
    {

    }
}
