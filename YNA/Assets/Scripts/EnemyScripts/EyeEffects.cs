#region Header
//*************************************************
// Project: We're Tethered Together
// File: EyeEffects.cs
// Author/s: Emmy Berg
//
// Desc: Controls post processing / special effects
//       for the eye states
//
// Notes:
//  + DEPRECIATED
//
// Last Edit: 5/11/2023
//
//*************************************************
#endregion

// !*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!
// ////////////////////////////////////////////////////////////////////////////
// DEPRECIATED
// ///////////////////////////////////////////////////////////////////////////
// !*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!*+!


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;


public class EyeEffects : MonoBehaviour
{
    #region DEPRECIATED
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    ActivateEyes eyeController;
    CinemachineImpulseSource impulse;


    Volume globalVolume;
    VolumeProfile volProf;
    Bloom bloom;
    Vignette vignette;
    ChromaticAberration chromatic;
    FilmGrain filmGrain;
    LiftGammaGain liftGammaGain;

    bool inState = false;
    bool canShake = true;
    bool goToSleep = false;

    [SerializeField]
    float bloomStartVal = 0f;
    [SerializeField] 
    float vignetteStartVal = 0.4f;
    [SerializeField]
    float chromaticStartVal = 0f;
    [SerializeField]
    float grainStartVal = 0f;
    //[SerializeField]
    //float gammaStartVal = 0f;

    [SerializeField]
    float bloomGoalVal = 5.3f;
    [SerializeField]
    float vignetteGoalVal = 0.675f;
    [SerializeField]
    float chromaticGoalVal = 0.55f;
    [SerializeField]
    float grainGoalVal = 0.875f;
    [SerializeField]
    float gammaGoalVal = 0.025f;
// *********************************************************************


////////////////////////////////////////////////////////////////////////
// AWAKE ===============================================================
    void Awake()
    {
        impulse = GetComponent<CinemachineImpulseSource>();

        eyeController = GameObject.FindGameObjectWithTag("EyeManager")?.GetComponent<ActivateEyes>();

        if (eyeController)
        {
            globalVolume = GetComponent<Volume>();
            volProf = globalVolume.profile;

            volProf.TryGet<Bloom>(out bloom);
            volProf.TryGet<Vignette>(out vignette);
            volProf.TryGet<ChromaticAberration>(out chromatic);
            volProf.TryGet<FilmGrain>(out filmGrain);
            volProf.TryGet<LiftGammaGain>(out liftGammaGain);

            liftGammaGain.gamma.overrideState = false;
        }
    }


////////////////////////////////////////////////////////////////////////
// START ===============================================================
    void Start()
    {
        SleepFX();
    }


////////////////////////////////////////////////////////////////////////
// UPDATE ==============================================================
    void Update()
    {
        switch(eyeController.status)
        {
            case EyeStates.SLEEPING:
                if(eyeController.prevStatus == EyeStates.ACTIVE && goToSleep)
                {
                    GoToSleepFX();
                    goToSleep = false;
                }
                SleepFX();
                break;
            case EyeStates.WAKING:
                WakingFX();
                break;
            case EyeStates.ACTIVE:
                ActiveFX();
                break;
            case EyeStates.SEEN:
                SeenFX();
                break;
        }
    }


////////////////////////////////////////////////////////////////////////
// SLEEP FX ============================================================
// Special effects for when eye is in "sleeping state"
    void SleepFX()
    {
        bloom.dirtIntensity.Override(bloomStartVal);
        vignette.intensity.Override(vignetteStartVal);
        chromatic.intensity.Override(chromaticStartVal);
        filmGrain.intensity.Override(grainStartVal);
        liftGammaGain.lift.Override(new Vector4(0,0,0,0));
        liftGammaGain.gain.Override(new Vector4(0,0,0,0));

        inState = false;
        canShake = true;
    }


////////////////////////////////////////////////////////////////////////
// GO TO SLEEP FX ======================================================
// Special effects for when eye transitions back to sleep from active
    void GoToSleepFX()
    {
        if (inState == false)
        {
            inState = true;
            StartCoroutine(Lerp(bloom.dirtIntensity, bloomGoalVal, bloomStartVal, 0.25f));
            StartCoroutine(Lerp(chromatic.intensity, chromaticGoalVal, chromaticStartVal, 0.25f));
            StartCoroutine(Lerp(vignette.intensity, vignetteGoalVal, vignetteStartVal, 0.025f));
            StartCoroutine(Lerp(liftGammaGain.lift, gammaGoalVal, 0, 0.25f));
            StartCoroutine(Lerp(liftGammaGain.gain, gammaGoalVal, 0, 0.25f));
        }

        if (canShake == false)
        {
            canShake = true;
            CameraShake.manager.Shake(impulse, 0.15f);
        }
    }


////////////////////////////////////////////////////////////////////////
// WAKING FX ===========================================================
// Special effects for when eye is beginning to wake
    void WakingFX()
    {
        if (inState == false)
        {
            inState = true;

            StartCoroutine(Lerp(chromatic.intensity, chromaticStartVal, chromaticGoalVal, eyeController.wakingTime));
            StartCoroutine(Lerp(vignette.intensity, vignetteStartVal, vignetteGoalVal, eyeController.wakingTime));
            StartCoroutine(Lerp(bloom.dirtIntensity, bloomStartVal, bloomGoalVal, eyeController.wakingTime));
            StartCoroutine(Lerp(liftGammaGain.lift, 0, gammaGoalVal, eyeController.wakingTime));
            StartCoroutine(Lerp(liftGammaGain.gain, 0, gammaGoalVal, eyeController.wakingTime));
        }
    }


////////////////////////////////////////////////////////////////////////
// ACTIVE FX ===========================================================
// Special effects for when eye is currently active
    void ActiveFX()
    {
        StopAllCoroutines();
        inState = false;
        if(canShake)
        {
            canShake = false;
            CameraShake.manager.Shake(impulse, 0.15f);
        }    
        // TODO: put in lerp function
        chromatic.intensity.Override(0.85f);
        bloom.dirtIntensity.Override(bloomGoalVal);
        vignette.intensity.Override(vignetteGoalVal);
        goToSleep = true;
    }


////////////////////////////////////////////////////////////////////////
// SEEN FX =============================================================
// Special effects for when eye has seen player
    void SeenFX()
    {
        CameraShake.manager.Shake(impulse, 0.15f);
        chromatic.intensity.Override(1);
        filmGrain.intensity.Override(grainGoalVal);
        bloom.dirtIntensity.Override(bloomGoalVal + 8.5f);
    }


////////////////////////////////////////////////////////////////////////
// LERP ================================================================
// Interpolates [param] from [start] to [end] val, within the duration
// of [time] seconds.
// [param] should either be a float or vec4 parameter
    IEnumerator Lerp(object param, float start, float end, float time)
    {
        float timeElapsed = 0;

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

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
    #endregion
}
