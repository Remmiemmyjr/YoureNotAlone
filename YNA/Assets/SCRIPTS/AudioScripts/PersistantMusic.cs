//*************************************************
// Project: We're Tethered Together
// File: PersistantMusic.cs
// Author/s: Corbyn LaMar
//
// Desc: Manage persistant music through level
//       transitions
//
// Notes:
// -
//
// Last Edit: 8/15/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public enum MusicFiles
{
    musicNone,
    musicMenuLoop,
    musicAlone1Loop,
    musicAlone2Loop,
    musicBasicLoop,
    musicSlowLoop,
    musicWinLoop,
    musicCount
}

public class PersistantMusic : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    // Static instance of the PersistantMusic to check if we should keep or delete new instances
    static PersistantMusic playerInstance;
    
    // The tracks of the music player within different audio sources
    public AudioSource audioMenu;
    public AudioSource audioBasic;
    public AudioSource audioAlone1;
    public AudioSource audioAlone2;
    public AudioSource audioSlow;
    public AudioSource audioWin;

    private AudioSource currentSource;
    private MusicFiles currentFile;
    public AudioMixer audioMixer;

    private float storedPauseVolume;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    private void Start()
    {
        // Check if we have a persistant music player
        if (PersistantMusic.playerInstance == null)
        {
            // Dont destroy this instance
            DontDestroyOnLoad(this);

            // Save a reference to this instance in the static variable
            PersistantMusic.playerInstance = this;

            // Track scene changes
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

            // Play menu on first load
            Start_Stop_Music(MusicFiles.musicMenuLoop);

            // Store the pause volume for later use
            storedPauseVolume = currentSource.volume;
        }
        else
        {
            // Destroy extra instances
            Destroy(gameObject);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // START STOP MUSIC ====================================================
    void Start_Stop_Music(MusicFiles music_file)
    {
        if (music_file == currentFile)
            return;

        // Start transition
        LerpAudioOut(0.25f);

        if (music_file == MusicFiles.musicMenuLoop)
        {
            currentFile = MusicFiles.musicMenuLoop;

            // Stop other sources
            audioAlone1.Stop();
            audioAlone2.Stop();
            audioBasic.Stop();
            audioSlow.Stop();
            audioWin.Stop();

            // Start if not already playing
            if (audioMenu.isPlaying == false)
            {
                audioMenu.Play();
                currentSource = audioMenu;
            }
        }
        else if (music_file == MusicFiles.musicAlone1Loop)
        {
            currentFile = MusicFiles.musicAlone1Loop;

            // Stop other sources
            audioAlone2.Stop();
            audioBasic.Stop();
            audioMenu.Stop();
            audioSlow.Stop();
            audioWin.Stop();

            // Start if not already playing
            if (audioAlone1.isPlaying == false)
            {
                audioAlone1.Play();
                currentSource = audioAlone1;
            }
        }
        else if (music_file == MusicFiles.musicAlone2Loop)
        {
            currentFile = MusicFiles.musicAlone2Loop;

            // Stop other sources
            audioAlone1.Stop();
            audioBasic.Stop();
            audioMenu.Stop();
            audioSlow.Stop();
            audioWin.Stop();

            // Start if not already playing
            if (audioAlone2.isPlaying == false)
            {
                audioAlone2.Play();
                currentSource = audioAlone2;
            }
        }
        else if (music_file == MusicFiles.musicBasicLoop)
        {
            currentFile = MusicFiles.musicBasicLoop;

            // Stop other sources
            audioAlone1.Stop();
            audioAlone2.Stop();
            audioMenu.Stop();
            audioSlow.Stop();
            audioWin.Stop();

            // Start if not already playing
            if (audioBasic.isPlaying == false)
            {
                audioBasic.Play();
                currentSource = audioBasic;
            }
        }
        else if (music_file == MusicFiles.musicSlowLoop)
        {
            currentFile = MusicFiles.musicSlowLoop;

            // Stop other sources
            audioAlone1.Stop();
            audioAlone2.Stop();
            audioBasic.Stop();
            audioMenu.Stop();
            audioWin.Stop();

            // Start if not already playing
            if (audioSlow.isPlaying == false)
            {
                audioSlow.Play();
                currentSource = audioSlow;
            }
        }
        else if (music_file == MusicFiles.musicWinLoop)
        {
            currentFile = MusicFiles.musicWinLoop;

            // Stop other sources
            audioAlone1.Stop();
            audioAlone2.Stop();
            audioBasic.Stop();
            audioMenu.Stop();
            audioSlow.Stop();

            // Start if not already playing
            if (audioWin.isPlaying == false)
            {
                audioWin.Play();
                currentSource = audioWin;
            }
        }

        // End transition
        LerpAudioIn(0.25f);
    }


    ////////////////////////////////////////////////////////////////////////
    // ON SCENE LOADED =====================================================
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode load_scene_mode)
    {
        // Reset Audio Volumes and pause effects
        audioAlone1.volume = 1.0f;
        audioAlone2.volume = 1.0f;
        audioMenu.volume = 1.0f;
        audioBasic.volume = 1.0f;
        audioSlow.volume = 1.0f;
        audioWin.volume = 1.0f;

        ApplyPauseEffects(false);

        // Check the name of the scene for music play changes
        switch (scene.name)
        {
            case ("MainMenu"):
                Start_Stop_Music(MusicFiles.musicMenuLoop);
                break;
            case ("Tutorial-1"):
                Start_Stop_Music(MusicFiles.musicAlone1Loop);
                break;
            case ("Tutorial-2"):
                Start_Stop_Music(MusicFiles.musicAlone1Loop);
                break;
            case ("Tutorial-Transition"):
                Start_Stop_Music(MusicFiles.musicAlone1Loop);
                break;
            case ("Level-5"):
                Start_Stop_Music(MusicFiles.musicAlone2Loop);
                break;
            case ("GameOver"):
                Start_Stop_Music(MusicFiles.musicWinLoop);
                break;
            default:
                Start_Stop_Music(MusicFiles.musicBasicLoop);
                break;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // LERP AUDIO OUT ======================================================
    public IEnumerator LerpAudioOut(float time)
    {
        float timeElapsed = 0;
        float startVolume = currentSource.volume;

        while (timeElapsed <= time)
        {
            float valueToLerp = Mathf.Lerp(startVolume, 0.0f, timeElapsed / time);

            // Debug.Log("Music:" + valueToLerp);

            currentSource.volume = valueToLerp;

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // LERP AUDIO IN =======================================================
    public IEnumerator LerpAudioIn(float time)
    {
        float timeElapsed = 0;

        while (timeElapsed <= time)
        {
            float valueToLerp = Mathf.Lerp(0.0f, 1.0f, timeElapsed / time);

            // Debug.Log("Music:" + valueToLerp);

            currentSource.volume = valueToLerp;

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // APPLY PAUSE FX ======================================================
    public void ApplyPauseEffects(bool isPaused)
    {
        if (isPaused)
        {
            // Store the volume and lay music over pause menu
            storedPauseVolume = currentSource.volume;
            currentSource.volume = 1.0f;

            GetComponent<AudioLowPassFilter>().enabled = true;
            GetComponent<AudioHighPassFilter>().enabled = true;
        }
        else
        {
            // Restore music levels after pause
            currentSource.volume = storedPauseVolume;

            GetComponent<AudioLowPassFilter>().enabled = false;
            GetComponent<AudioHighPassFilter>().enabled = false;
        }
    }
}
