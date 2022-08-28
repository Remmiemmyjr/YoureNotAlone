using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuAudioController : MonoBehaviour
{
    public float fadeTime;
    public float targetLevel;
    public string mixerGroupExposedParameter;

    // Start is called before the first frame update
    void Start()
    {
        // Begin music fade-in.
        StartCoroutine(FadeMixerGroup.StartFade(GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer,
            mixerGroupExposedParameter, fadeTime, targetLevel));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
