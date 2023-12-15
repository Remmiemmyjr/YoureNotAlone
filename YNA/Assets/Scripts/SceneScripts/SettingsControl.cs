//*************************************************
// Project: We're Tethered Together
// File: SettingsControl.cs
// Author/s: Corbyn LaMar
//           Mike Doeren
//
// Desc: Manage option modifiers
//
// Notes:
// -
//
// Last Edit: 7/23/2023
//
//*************************************************

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    float currentRefreshRate;
    List<Resolution> filteredResolutions = new List<Resolution>();

    VolumeProfile volProf;
    LiftGammaGain liftGammaGain;
    Bloom bloom;
    public float bloomDefaultIntensity = 0.65f;
    [HideInInspector]
    public static float myBloomVal;

    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider brightnessSlider;
    public Toggle bloomToggle;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    private void Awake()
    {
        // Update volume and brightness for scene
        volProf = GameObject.FindWithTag("GlobalVolume").GetComponent<Volume>().profile;
        volProf.TryGet(out liftGammaGain);
        volProf.TryGet(out bloom);

        float gammaVal = PlayerPrefs.GetFloat("Gamma");
        liftGammaGain.gamma.Override(new Vector4(0, 0, 0, gammaVal));

        int bloomToggleVal = PlayerPrefs.GetInt("BloomToggle");

        if (bloomToggleVal == 1)
        {
            bloom.intensity.value = bloomDefaultIntensity + (gammaVal * 0.1f);
            bloomToggle.isOn = true;
        }
        else
        {
            bloom.intensity.value = 0;
            bloomToggle.isOn = false;
        }
        myBloomVal = bloom.intensity.value;

        // Update Slider UI
        float musicVol = 0.0f;
        audioMixer.GetFloat("MusicVolume", out musicVol);

        float sfxVol = 0.0f;
        audioMixer.GetFloat("SFXVolume", out sfxVol);

        musicVol = Mathf.Pow(10, musicVol / 20);
        sfxVol = Mathf.Pow(10, sfxVol / 20);

        musicVolumeSlider.value = musicVol;
        sfxVolumeSlider.value = sfxVol;
        brightnessSlider.value = gammaVal;

        gameObject.SetActive(false);
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        List<string> options = new List<string>();
        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        int currentResolutionIndex = 0;
        resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; ++i)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        // Loop through all available screen resolutions and add them into options list
        for (int i = 0; i < filteredResolutions.Count; ++i)
        {
            string option = filteredResolutions[i].width + " x " + filteredResolutions[i].height;
            options.Add(option);

            // Check to see if the current resolution matches the player's
            if (filteredResolutions[i].width == Screen.currentResolution.width &&
                filteredResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Then add our resolutions to our resolution dropdown
        resolutionDropdown.AddOptions(options);

        // Set the resolution here
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }


    ////////////////////////////////////////////////////////////////////////
    // SET RESOLUTION ======================================================
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    ////////////////////////////////////////////////////////////////////////
    // SET MUSIC VOLUME ====================================================
    public void SetMusicVolume(float musicVolume)
    {
        float sliderToDB = Mathf.Log10(musicVolume) * 20;

        audioMixer.SetFloat("MusicVolume", sliderToDB);

        PlayerPrefs.SetFloat("MusicVolume", sliderToDB);
        PlayerPrefs.Save();
    }


    ////////////////////////////////////////////////////////////////////////
    // SET SFX VOLUME ======================================================
    public void SetSFXVolume(float sfxVolume)
    {
        float sliderToDB = Mathf.Log10(sfxVolume) * 20;

        audioMixer.SetFloat("SFXVolume", sliderToDB);

        PlayerPrefs.SetFloat("SFXVolume", sliderToDB);
        PlayerPrefs.Save();
    }


    ////////////////////////////////////////////////////////////////////////
    // SET FULLSCREEN ======================================================
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }


    ////////////////////////////////////////////////////////////////////////
    // SET GAMMA ===========================================================
    // Currently used to set the brightness value
    public void SetGamma(float gammaVal)
    {
        liftGammaGain.gamma.Override(new Vector4(0, 0, 0, gammaVal));
        if (bloomToggle.isOn)
        {
            bloom.intensity.value = bloomDefaultIntensity + (gammaVal * 0.1f);
        }
        else
        {
            bloom.intensity.value = 0;
        }
        myBloomVal = bloom.intensity.value;

        PlayerPrefs.SetFloat("Gamma", gammaVal);
        PlayerPrefs.Save();
    }


    ////////////////////////////////////////////////////////////////////////
    // TOGGLE BLOOM ===========================================================
    // Currently used to toggle bloom
    public void ToggleBloom()
    {
        if (bloomToggle.isOn)
            PlayerPrefs.SetInt("BloomToggle", 1);
        else
            PlayerPrefs.SetInt("BloomToggle", 0);

        SetGamma(PlayerPrefs.GetFloat("Gamma"));
    }

}
