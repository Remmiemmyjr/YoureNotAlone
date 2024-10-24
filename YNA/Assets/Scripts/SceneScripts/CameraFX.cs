//*************************************************
// Project: We're Tethered Together
// File: EyeEffects.cs
// Author/s: Cameron Myers
//           Emmy Berg
//
// Desc: Controls post processing / special effects
//       for the eye states
//
// Last Edit: 7/2/2023
//
//*************************************************

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
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public string comment;

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
    public bool vignette_instant = false;

    public float vignette_start_val = 0.0f;
    public float vignette_goal_val = 0.0f;
    public float vignette_lerp_time = 0.0f;


    //bloom vars
    public bool bloom_toggle = false;
    public bool bloom_instant = false;

    public float bloom_start_val = 0.0f;
    public float bloom_goal_val = 0.0f;
    public float bloom_lerp_time = 0.0f;


    //chromatic abberation values
    public bool chromab_toggle = false;
    public bool chromab_instant = false;

    public float chromab_start_val = 0.0f;
    public float chromab_goal_val = 0.0f;
    public float chromab_lerp_time = 0.0f;


    //film grain values
    public bool flmgrn_toggle = false;
    public bool flmgrn_instant = false;

    public float flmgrn_start_val = 0.0f;
    public float flmgrn_goal_val = 0.0f;
    public float flmgrn_lerp_time = 0.0f;

    //lift 
    public bool lift_toggle = false;
    public bool lift_instant = false;

    public float lift_start_val = 0.0f;
    public float lift_goal_val = 0.0f;
    public float lift_lerp_time = 0.0f;

    public bool gamma_toggle = false;
    public bool gamma_instant = false;

    public float gamma_start_val = 0.0f;
    public float gamma_goal_val = 0.0f;
    public float gamma_lerp_time = 0.0f;

    public bool gain_toggle = false;
    public bool gain_instant = false;

    public float gain_start_val = 0.0f;
    public float gain_goal_val = 0.0f;
    public float gain_lerp_time = 0.0f;

    float bloomDefaultThreshold = 0.725f;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    // Move to PPManager
    void Awake()
    {
        globalVolume = gameObject.GetComponent<PostProcessingManager>().GetGlobalVolume();
        volProf = globalVolume.profile;

        volProf.TryGet<Bloom>(out bloom);
        volProf.TryGet<Vignette>(out vignette);
        volProf.TryGet<ChromaticAberration>(out chromatic);
        volProf.TryGet<FilmGrain>(out filmGrain);
        volProf.TryGet<LiftGammaGain>(out liftGammaGain);

        liftGammaGain.gamma.overrideState = false;

        float gammaVal = PlayerPrefs.GetFloat("Gamma");
        liftGammaGain.gamma.Override(new Vector4(0, 0, 0, gammaVal));
        bloom.threshold.value = bloomDefaultThreshold + (gammaVal * 0.1f);
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        
    }

    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
    }


    ////////////////////////////////////////////////////////////////////////
    // ACTIVATE ============================================================
    public void Activate()
    {
        //stop any active lerps
        StopAllCoroutines();

        //each if is for activation
        if (vignette_toggle)
        {
            if (vignette_instant)
            {
                vignette.intensity.Override(vignette_goal_val);
            }
            else
            {
                vignette.intensity.Override(vignette_start_val);
                StartCoroutine(Lerp(vignette.intensity, vignette_start_val, vignette_goal_val, vignette_lerp_time));
            }
        }

        if (bloom_toggle)
        {
            if (bloom_instant)
            {
                bloom.intensity.Override(bloom_goal_val);
            }
            else
            {
                bloom.intensity.Override(bloom_start_val);
                StartCoroutine(Lerp(bloom.intensity, bloom_start_val, bloom_goal_val, bloom_lerp_time));
            }
        }

        if (chromab_toggle)
        {
            if (chromab_instant)
            {
                chromatic.intensity.Override(chromab_goal_val);
            }
            else
            {
                chromatic.intensity.Override(chromab_start_val);
                StartCoroutine(Lerp(chromatic.intensity, chromab_start_val, chromab_goal_val, chromab_lerp_time));
            }
        }

        if (flmgrn_toggle)
        {
            if (flmgrn_instant)
            {
                filmGrain.intensity.Override(flmgrn_goal_val);
            }
            else
            {
                filmGrain.intensity.Override(flmgrn_start_val);
                StartCoroutine(Lerp(filmGrain.intensity, flmgrn_start_val, flmgrn_goal_val, flmgrn_lerp_time));
            }
        }

        if (lift_toggle)
        {
            if (lift_instant)
            {
                liftGammaGain.lift.Override(new Vector4(0, 0, 0, lift_goal_val));
            }
            else
            {
                liftGammaGain.lift.Override(new Vector4(0, 0, 0, lift_start_val));
                StartCoroutine(Lerp(liftGammaGain.lift, lift_start_val, lift_goal_val, lift_lerp_time));
            }
        }

        if (gamma_toggle)
        {
            if (gamma_instant)
            {
                liftGammaGain.gamma.Override(new Vector4(0, 0, 0, gamma_goal_val));
            }
            else
            {
                liftGammaGain.gamma.Override(new Vector4(0, 0, 0, gamma_start_val));
                StartCoroutine(Lerp(liftGammaGain.gamma, gamma_start_val, gamma_goal_val, gamma_lerp_time));
            }
        }

        if (gain_toggle)
        {
            if (gain_instant)
            {
                liftGammaGain.gain.Override(new Vector4(0, 0, 0, gain_goal_val));
            }
            else
            {
                liftGammaGain.gain.Override(new Vector4(0, 0, 0, gain_start_val));
                StartCoroutine(Lerp(liftGammaGain.gain, gain_start_val, gain_goal_val, gain_lerp_time));
            }
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
        float valueToLerp = 0.0f;

        while (timeElapsed <= time)
        {
            if (!Info.isPaused)
                valueToLerp = Mathf.Lerp(start, end, timeElapsed / time);

            switch (param)
            {
                case ClampedFloatParameter cfp:
                    cfp.Override(valueToLerp);
                    break;
                case Vector4Parameter v4p:
                    v4p.Override(new Vector4(0, 0, 0, -valueToLerp));
                    break;
            }

            if (!Info.isPaused)
                timeElapsed += Time.unscaledDeltaTime;

            yield return null;
        }
    }
}
