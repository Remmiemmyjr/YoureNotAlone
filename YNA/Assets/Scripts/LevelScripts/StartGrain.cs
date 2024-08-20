// Project: We're Tethered Together
// File: SteamAchievementAwarder.cs
// Author/s: Emmy Berg
//
// Desc: Add grain to levels where player is alone
//
// Notes:
//  - 
//
// Last Edit: 7/2/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class StartGrain : MonoBehaviour
{
    Volume globalVolume;
    VolumeProfile volProf;
    FilmGrain filmGrain;

    [SerializeField]
    private float grainStartVal;

    private void Awake()
    {
        globalVolume = GetComponent<Volume>();
        volProf = globalVolume.profile;

        volProf.TryGet<FilmGrain>(out filmGrain);
    }

    // Start is called before the first frame update
    void Start()
    {
        filmGrain.intensity.Override(grainStartVal);
    }
}
