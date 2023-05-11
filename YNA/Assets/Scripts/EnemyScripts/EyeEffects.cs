using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;

enum VFX
{
    Bloom,
    Vignette,
    ChromaticAberration,
    FilmGrain,
    LiftGammaGain
}

public class EyeEffects : MonoBehaviour
{
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

    [SerializeField]
    float bloomStartVal = 0f;
    [SerializeField] 
    float vignetteStartVal = 0.4f;
    [SerializeField]
    float chromaticStartVal = 0f;
    [SerializeField]
    float grainStartVal = 0f;
    [SerializeField]
    float gammaStartVal = 0f;

    [SerializeField]
    float bloomGoalVal = 5.3f;
    [SerializeField]
    float vignetteGoalVal = 0.6f;
    [SerializeField]
    float chromaticGoalVal = 0.55f;
    [SerializeField]
    float grainGoalVal = 0.875f;
    [SerializeField]
    float gammaGoalVal = 0.975f;

    delegate void effect(float x);


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

    void Start()
    {
        SleepFX();
    }


    void Update()
    {
        switch(eyeController.status)
        {
            case EyeStates.SLEEPING:
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

    void SleepFX()
    {
        bloom.dirtIntensity.Override(bloomStartVal);
        vignette.intensity.Override(vignetteStartVal);
        chromatic.intensity.Override(chromaticStartVal);
        filmGrain.intensity.Override(grainStartVal);
        liftGammaGain.lift.Override(new Vector4(0,0,0, 0));
        liftGammaGain.gain.Override(new Vector4(0,0,0, 0));

        inState = false;
        canShake = true;
    }

    void GoToSleepFX()
    {

    }

    void WakingFX()
    {
        if (inState == false)
        {
            inState = true;
            StartCoroutine(Lerp(VFX.ChromaticAberration, chromaticStartVal, chromaticGoalVal, eyeController.wakingTime, 1000));
            StartCoroutine(Lerp(VFX.Vignette, vignetteStartVal, vignetteGoalVal, eyeController.wakingTime, 7500));
            StartCoroutine(Lerp(VFX.Bloom, bloomStartVal, bloomGoalVal, eyeController.wakingTime, 5000));
            StartCoroutine(Lerp(VFX.LiftGammaGain, 0, gammaGoalVal, eyeController.wakingTime, 5000));
        }
    }

    void ActiveFX()
    {
        StopAllCoroutines();
        if(canShake)
        {
            canShake = false;
            CameraShake.manager.Shake(impulse);
        }    
        chromatic.intensity.Override(0.85f);
        bloom.dirtIntensity.Override(bloomGoalVal);
    }

    void SeenFX()
    {
        CameraShake.manager.Shake(impulse);
        chromatic.intensity.Override(1);
        filmGrain.intensity.Override(grainGoalVal);
        bloom.dirtIntensity.Override(bloomGoalVal + 5f);
    }

    IEnumerator Lerp(VFX type, float start, float end, float time, float multiplier)
    {
        float timeElapsed = 0;

        while(timeElapsed < time)
        {
            float valueToLerp = Mathf.Lerp(start, end, timeElapsed / time);

            switch(type)
            {
                case VFX.Bloom:
                    bloom.dirtIntensity.Override(valueToLerp * 2f);
                    break;
                case VFX.Vignette:
                    vignette.intensity.Override(valueToLerp * 1.05f);
                    break;
                case VFX.ChromaticAberration:
                    chromatic.intensity.Override(valueToLerp);
                    break;
                case VFX.FilmGrain:
                    filmGrain.intensity.Override(valueToLerp);
                    break;
                case VFX.LiftGammaGain:
                    liftGammaGain.lift.Override(new Vector4(0, 0, 0, -valueToLerp * 0.025f));
                    liftGammaGain.gain.Override(new Vector4(0, 0, 0, -valueToLerp * 0.025f));
                    Debug.Log(valueToLerp);
                    break;
            }

            timeElapsed += Time.deltaTime;

            yield return new WaitForSeconds(time/multiplier);
        }

        //valueToLerp = end;
    }
}
