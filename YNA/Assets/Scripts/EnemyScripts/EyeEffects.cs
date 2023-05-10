using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EyeEffects : MonoBehaviour
{
    ActivateEyes eyeController;

    Volume globalVolume;
    VolumeProfile volProf;
    Bloom bloom;
    Vignette vignette;
    ChromaticAberration chromatic;
    FilmGrain filmGrain;
    LiftGammaGain liftGammaGain;

    float valueToLerp1;
    float valueToLerp2;

    bool inState = false;

    [SerializeField]
    float bloomStartVal = 0f;
    [SerializeField] 
    float vignetteStartVal = 0.4f;
    [SerializeField]
    float chromaticStartVal = 0f;
    [SerializeField]
    float grainStartVal = 0f;
    [SerializeField]
    float gammaStartVal = 1f;

    [SerializeField]
    float bloomGoalVal = 5f;
    [SerializeField]
    float vignetteGoalVal = 0.5f;
    [SerializeField]
    float chromaticGoalVal = 0.55f;
    [SerializeField]
    float grainGoalVal = 0.875f;
    [SerializeField]
    float gammaGoalVal = 0.97f;


    void Awake()
    {
        eyeController = GameObject.FindGameObjectWithTag("EyeManager")?.GetComponent<ActivateEyes>();

        if (eyeController)
        {
            globalVolume = GetComponent<Volume>();
            volProf = globalVolume.profile;
            //bloom = globalVolume.GetComponent<Bloom>();
            //vignette = globalVolume.GetComponent<Vignette>();
            //chromatic = globalVolume.GetComponent<ChromaticAberration>();
            //filmGrain = globalVolume.GetComponent<FilmGrain>();
            //liftGammaGain = globalVolume.GetComponent<LiftGammaGain>();
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
    }

    void WakingFX()
    {
        if (inState == false)
        {
            inState = true;
            StartCoroutine(LerpChromatic(chromaticStartVal, chromaticGoalVal, eyeController.wakingTime));
        }

        //StartCoroutine(LerpGamma(gammaStartVal, gammaGoalVal, eyeController.wakingTime));
        //liftGammaGain.lift.Override(new Vector4(valueToLerp2, valueToLerp2, valueToLerp2, valueToLerp2));
        //liftGammaGain.gain.Override(new Vector4(valueToLerp2, valueToLerp2, valueToLerp2, valueToLerp2));
    }

    void ActiveFX()
    {
        StopAllCoroutines();
        chromatic.intensity.Override(1);
        filmGrain.intensity.Override(grainGoalVal);
        bloom.dirtIntensity.Override(bloomGoalVal);
    }

    IEnumerator LerpChromatic(float start, float end, float time)
    {
        float timeElapsed = 0;

        while(timeElapsed < time)
        {
            valueToLerp1 = Mathf.Lerp(start, end, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            chromatic.intensity.Override(valueToLerp1);
            yield return new WaitForSeconds(time/40);
        }

        valueToLerp1 = end;
    }

    //IEnumerator LerpGamma(float start, float end, float time)
    //{
    //    float timeElapsed = 0;

    //    while (timeElapsed < time)
    //    {
    //        valueToLerp2 = Mathf.Lerp(start, end, timeElapsed / time);
    //        timeElapsed += Time.deltaTime;
    //        yield return valueToLerp2;
    //    }

    //    valueToLerp2 = end;
    //}
}
