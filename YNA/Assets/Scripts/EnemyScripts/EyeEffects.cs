using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EyeEffects : MonoBehaviour
{
    ActivateEyes eyeController;

    PostProcessVolume globalVolume;
    Vignette vignette;
    ChromaticAberration chromatic;
    Grain filmGrain;
    ColorGrading liftGammaGain;

    [SerializeField] 
    float vignetteStartVal = 0.4f;
    [SerializeField]
    float chromaticStartVal = 0f;
    [SerializeField]
    float grainStartVal = 0f;

    [SerializeField]
    float vignetteGoalVal = 0.6f;
    [SerializeField]
    float chromaticGoalVal = 0.55f;
    [SerializeField]
    float grainGoalVal = 0.875f;

    void Awake()
    {
        eyeController = GameObject.FindGameObjectWithTag("EyeManager")?.GetComponent<ActivateEyes>();

        globalVolume = GetComponent<PostProcessVolume>();
        vignette = globalVolume.GetComponent<Vignette>();
        chromatic = globalVolume.GetComponent<ChromaticAberration>();
        filmGrain = globalVolume.GetComponent<Grain>();
        liftGammaGain = globalVolume.GetComponent<ColorGrading>();
    }

    void Start()
    {
        vignette.intensity.Override(vignetteStartVal);
        chromatic.intensity.Override(chromaticStartVal);
        filmGrain.intensity.Override(grainStartVal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
