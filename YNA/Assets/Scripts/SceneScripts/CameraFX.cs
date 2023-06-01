using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    Volume globalVolume;
    VolumeProfile volProf;
    Bloom bloom;
    Vignette vignette;
    ChromaticAberration chromatic;
    FilmGrain filmGrain;
    LiftGammaGain liftGammaGain;


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

    /// <summary>
    /// Activate post processing based on stored parameters 
    /// </summary>
    public void Activate()
    {
        //stop any active lerps
        StopAllCoroutines();

        //each if is for activation
        if (vignette_toggle)
        {
            StartCoroutine(Lerp(vignette.intensity, vignette_start_val, vignette_goal_val, vignette_lerp_time));

        }


        if (bloom_toggle)
        {
            StartCoroutine(Lerp(bloom.dirtIntensity, bloom_start_val, bloom_goal_val, bloom_lerp_time));

        }


        if (chromab_toggle)
        {
            StartCoroutine(Lerp(chromatic.intensity, chromab_start_val, chromab_goal_val, chromab_lerp_time));

        }


        if (flmgrn_toggle)
        {
            StartCoroutine(Lerp(filmGrain.intensity, flmgrn_start_val, flmgrn_goal_val, flmgrn_lerp_time));


        }

    }


    ////////////////////////////////////////////////////////////////////////
    // LERP ================================================================
    // Interpolates [param] from [start] to [end] val, within the duration
    // of [time] seconds.
    // [param] should either be a float or vec4 parameter
    IEnumerator Lerp(object param, float start, float end, float time)
    {
        float timeElapsed = 0;
        float timeDelta = time / 200;

        while (timeElapsed <= time)
        {
            float valueToLerp = Mathf.Lerp(start, end, timeElapsed / time);

            switch (param)
            {
                case ClampedFloatParameter cfp:
                    cfp.Override(valueToLerp);
                    break;
                case Vector4Parameter v4p:
                    v4p.Override(new Vector4(0, 0, 0, -valueToLerp));
                    break;
            }

            timeElapsed += timeDelta;

            yield return new WaitForSeconds(timeDelta);
        }
    }
}
