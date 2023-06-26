using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LevelMusicController : MonoBehaviour
{
    public string mixerGroupExposedParameter;
    public float startingVolume;

    // Start is called before the first frame update
    void Start()
    {
        AudioMixer mixerGroup = GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;
        mixerGroup.SetFloat(mixerGroupExposedParameter, startingVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
