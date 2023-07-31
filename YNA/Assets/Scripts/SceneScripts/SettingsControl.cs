using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    float currentRefreshRate;
    List<Resolution> filteredResolutions = new List<Resolution>();

    VolumeProfile volProf;
    LiftGammaGain liftGammaGain;
    Bloom bloom;
    public float bloomDefaultThreshold = 0.725f;

    public Slider volumeSlider;
    public Slider brightnessSlider;


    private void Awake()
    {
        // Update volume and brightness for scene
        volProf = GameObject.FindWithTag("GlobalVolume").GetComponent<Volume>().profile;
        volProf.TryGet(out liftGammaGain);
        volProf.TryGet(out bloom);

        float gammaVal = PlayerPrefs.GetFloat("Gamma");
        liftGammaGain.gamma.Override(new Vector4(0, 0, 0, gammaVal));
        bloom.threshold.value = bloomDefaultThreshold + (gammaVal * 0.1f);

        // Update Slider UI
        float masterVol = 0.0f;
        audioMixer.GetFloat("MasterVolume", out masterVol);

        volumeSlider.value = masterVol;
        brightnessSlider.value = gammaVal;
    }

    void Start()
    {
        List<string> options = new List<string>();
        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        int currentResolutionIndex = 0;
        resolutions = Screen.resolutions;

        //
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
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetGamma(float gammaVal)
    {
        liftGammaGain.gamma.Override(new Vector4(0, 0, 0, gammaVal));
        bloom.threshold.value = bloomDefaultThreshold + (gammaVal * 0.125f);

        PlayerPrefs.SetFloat("Gamma", gammaVal);
        PlayerPrefs.Save();
    }

}
