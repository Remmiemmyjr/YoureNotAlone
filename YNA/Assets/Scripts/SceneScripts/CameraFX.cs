using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[System.Serializable]
/*public class CameraFX_Event
{
    [SerializeField] private UnityEvent<CameraFX> fx_event = new UnityEvent<CameraFX>();
}*/

public class CameraFX : MonoBehaviour
{
    public enum FX
    {
        ChromaticAbberation,
        Vignette,
        Bloom,
        FilmGrain,
    }

    //vignette vars
    public bool vignette_toggle = false;

    public float vignette_start_val = 0.0f;
    public float vignette_goal_val = 0.0f;
    public float vignette_lerp_time = 0.0f;

    //bloom vars
    public bool bloom_toggle = false;

    public float bloom_start_val = 0.0f;
    public float bloom_goal_val = 0.0f;
    public float bloom_lerp_time = 0.0f;

    //chromatic abberation values
    public bool chromab_toggle = false;

    public float chromab_start_val = 0.0f;
    public float chromab_goal_val = 0.0f;
    public float chromab_lerp_time = 0.0f;

    //film grain values
    public bool flmgrn_toggle = false;

    public float flmgrn_start_val = 0.0f;
    public float flmgrn_goal_val = 0.0f;
    public float flmgrn_lerp_time = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void test(CameraFX fx)
    {
        Debug.Log(GetVignetteToggle());
    }

    public bool GetVignetteToggle()
    {
        return vignette_toggle;
    }
}
