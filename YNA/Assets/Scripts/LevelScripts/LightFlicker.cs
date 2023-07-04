//*************************************************
// Project: We're Tethered Together
// File: EyeEffects.cs
// Author/s: Corbyn Lamar
//
// Desc: Tool for creating light flicker / pulse
//
// Last Edit: 7/3/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    // Light component on the current object
    private UnityEngine.Rendering.Universal.Light2D currLight;

    // Radius
    [SerializeField]
    private bool flickerRadius = false;

    [SerializeField]
    private float lightFlickerRadiusRate = 1.0f;

    [SerializeField]
    private float radiusBase = 5.0f;

    [SerializeField]
    private float radiusHighEnd = 1.0f;

    // Intensity 
    [SerializeField]
    private bool flickerIntensity = false;

    [SerializeField]
    private float lightFlickerIntensityRate = 1.0f;

    [SerializeField]
    private float intensityBase = 5.0f;

    [SerializeField]
    private float intensityHighEnd = 1.0f;

    // Start is called before the first frame update
    void Awake()
    {
        currLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flickerRadius)
        {
            float lightRadiusValue = Mathf.Lerp(radiusBase, radiusHighEnd, Mathf.PingPong(Time.time / lightFlickerRadiusRate, 1));
            currLight.pointLightOuterRadius = lightRadiusValue;
        }

        if (flickerIntensity)
        {
            float lightIntensityValue = Mathf.Lerp(intensityBase, intensityHighEnd, Mathf.PingPong(Time.time / lightFlickerIntensityRate, 1));
            currLight.intensity = lightIntensityValue;
        }
    }
}
