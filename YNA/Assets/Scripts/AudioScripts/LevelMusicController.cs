//*************************************************
// Project: We're Tethered Together
// File: LevelMusicController.cs
// Author/s: K Preston
//
// Desc: Setup level music and mixer info
//
// Notes:
// -
//
// Last Edit: 7/16/2023
//
//*************************************************

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
}
