using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    AudioClip lastClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
