//*************************************************
// Project: We're Tethered Together
// File: PlayAudio.cs
// Author/s: K Preston
//
// Desc: Audio manager
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    AudioClip lastClip;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // GET RANDOM CLIP =====================================================
    public AudioClip GetRandomClip()
    {
        AudioClip clipToPlay = audioClipArray[Random.Range(0, audioClipArray.Length)];

        while (clipToPlay == lastClip)
        {
            clipToPlay = audioClipArray[Random.Range(0, audioClipArray.Length)];
        }

        lastClip = clipToPlay;
        return clipToPlay;
    }
}
