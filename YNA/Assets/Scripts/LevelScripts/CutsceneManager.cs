//*************************************************
// Project: We're Tethered Together
// File: CutsceneManager.cs
// Author/s: Corbyn LaMar
//           Emmy Berg
//
// Desc: Manages the cutscenes associated with the
//       current scene.
//
// Notes:
//  - Reworked the image indexing, bad index caused
//    levels with multiple cutscenes to crash
//
// Last Edit: 3/10/2024
//
//*************************************************

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Events;

public class CutsceneManager : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================

    public Component[] componentsToDisable;

    // Cutscene Items
    public Cutscene[] cutscenes;
    private Cutscene activeCutscene;

    private Image cutsceneCanvasFrame;
    private Image cutsceneCanvasTimer;

    private bool isCurrentlyPlaying = false;
    private int currentFrameIndex = 0;
    private float cutsceneTimer = 0.0f;
    public float holdFrame = 1.5f;

    private bool skipHold = false;
    private const float skipTheshold = 1f;
    private float skipTimer = 0.0f;

    private const float fadeTime = 1.0f;
    private bool inFade = false;

    private bool isStartCutscene = false;

    private PlayerInput inputManager;

    private AudioSource cutsceneAS;

    [SerializeField]
    public UnityEvent FadeToBlack;
    [SerializeField]
    public UnityEvent FadeBackIn;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // ON DESTROY ==========================================================
    // When this is destroyed, store what the "previous scene" was to avoid
    // cutscene repeats
    public static string PreviousLevel { get; private set; }
    // When this is destroyed, reset all cutscene play values
    private void OnDestroy()
    {
        PreviousLevel = gameObject.scene.name;
    }

    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        inputManager = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();

        if (PreviousLevel != gameObject.scene.name)
            ResetCutscenesPlayed();
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        // Get the canvas to play cutscenes on
        cutsceneCanvasFrame = transform.Find("Frame").GetComponent<Image>();
        cutsceneCanvasTimer = transform.Find("Timer").GetComponent<Image>();

        // Get the audio source to play the sound on
        cutsceneAS = GetComponent<AudioSource>();

        // If a cutscene is to play when the scene is loaded, then trigger it
        foreach (Cutscene currCutscene in cutscenes)
        {
            if (currCutscene.playOnSceneStart && PlayerPrefs.GetInt("CutPlayed_" + currCutscene.name) != 1)
            {
                // Hide transition
                GameObject.FindGameObjectWithTag("Transition").GetComponentInChildren<Image>().enabled = false;

                isStartCutscene = true;

                activeCutscene = currCutscene;

                PlayerPrefs.SetInt("CutPlayed_" + currCutscene.name, 1);

                StartCutscene();
                ProcessCutscenePreEffects();
            }
        }

        if (isStartCutscene == false)
        {
            GameObject.FindGameObjectWithTag("Transition").GetComponentInChildren<Animator>().SetTrigger("SceneStart");
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        // Make sure something is playing
        if (isCurrentlyPlaying && activeCutscene != null)
        {
            // Check for skip
            if (skipHold)
            {
                // Update Timer
                skipTimer += Time.deltaTime;

                // Update the skip timer UI
                if (cutsceneCanvasTimer)
                {
                    cutsceneCanvasTimer.fillAmount = skipTimer / skipTheshold;
                }

                // Check for skip end
                if (skipTimer > skipTheshold)
                {
                    // Reset timer
                    skipTimer = 0.0f;

                    // Finish and return
                    FinishCutscene();
                }
            }
            else
            {

                // Update cutscene time
                cutsceneTimer += Time.deltaTime;

                if (cutsceneTimer > fadeTime && cutsceneTimer < activeCutscene.timeBetween - 1)
                {
                    FadeToBlack.Invoke();

                    inFade = true;
                }

                // Check if the timer has elapsed
                if (cutsceneTimer >= activeCutscene.timeBetween)
                {
                    // Reset timer
                    cutsceneTimer = 0.0f;

                    // Advance or end cutscene
                    if (currentFrameIndex < activeCutscene.frames.Length - 1)
                    {
                        currentFrameIndex++;

                        cutsceneCanvasFrame.sprite = activeCutscene.frames[currentFrameIndex].frameImage;

                        FadeBackIn.Invoke();

                        inFade = false;
                    }
                    else
                    {
                        StartCoroutine(Load());
                    }
                }
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // LOAD ================================================================
    // Start FinishCutscene after delay...
    public IEnumerator Load()
    {
        yield return new WaitForSeconds(holdFrame);
        FinishCutscene();
    }


    ////////////////////////////////////////////////////////////////////////
    // START CUTSCENE ======================================================
    public void StartCutscene()
    {
        // Disable any components that are interfering with the cutscene
        for(int i = 0; i < componentsToDisable.Length; i++) 
        {
            componentsToDisable[i].gameObject.SetActive(false);
        }

        // No player input allowed
        inputManager.actions.Disable();

        // Update sprite
        cutsceneCanvasFrame.sprite = activeCutscene.frames[currentFrameIndex].frameImage;

        // Check if we have a cutscene music track to play
        if (activeCutscene.cutsceneTrack)
        {
            // Fade out normal music
            StartCoroutine(GameObject.FindWithTag("MusicController").GetComponent<PersistantMusic>().LerpAudioOut(1.5f));

            // Play the cutscene music
            cutsceneAS.clip = activeCutscene.cutsceneTrack;
            cutsceneAS.Play();
        }

        // Ensure they exist
        if (cutsceneCanvasFrame && cutsceneCanvasTimer)
        {
            // Turn on canvas image for cutscene play
            cutsceneCanvasFrame.enabled = true;
            cutsceneCanvasTimer.enabled = true;
        }

        // Update variables
        isCurrentlyPlaying = true;
    }


    ////////////////////////////////////////////////////////////////////////
    // PROCESS CUTSCENE ====================================================
    public void ProcessCutscenePreEffects()
    {
        activeCutscene.PreEvent.Invoke();
    }


    ////////////////////////////////////////////////////////////////////////
    // PROCESS CUTSCENE ====================================================
    public void ProcessCutscenePostEffects()
    {
        activeCutscene.PostEvent.Invoke();
    }


    ////////////////////////////////////////////////////////////////////////
    // FINISH CUTSCENE =====================================================
    public void FinishCutscene()
    {
        // Fade back in
        FadeBackIn.Invoke();
        // Re-enable player input
        inputManager.actions.Enable();

        // Process events
        ProcessCutscenePostEffects();

        // Update variables
        isCurrentlyPlaying = false;

        // Ensure they exist
        if (cutsceneCanvasFrame && cutsceneCanvasTimer)
        {
            // Turn off canvas image for cutscene
            cutsceneCanvasFrame.enabled = false;
            cutsceneCanvasTimer.enabled = false;
        }

        // Re-enable any components that were interfering with the cutscene
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].gameObject.SetActive(true);
        }

        // Stop Audio Source and fade music back in
        cutsceneAS.Stop();
        StartCoroutine(GameObject.FindWithTag("MusicController").GetComponent<PersistantMusic>().LerpAudioIn(0.1f));

        // If marked to be at the end of a scene, advance
        if (activeCutscene.nextSceneOnFinish)
        {
            // Reset active cutscene
            activeCutscene = null;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }
        else
        {
            // If coming from cutscene into the level, transition
            if (activeCutscene.playOnSceneStart)
            {
                // Re-show transition
                GameObject.FindGameObjectWithTag("Transition").GetComponentInChildren<Image>().enabled = true;

                // Play animation
                GameObject.FindGameObjectWithTag("Transition").GetComponentInChildren<Animator>().SetTrigger("SceneStart");
            }

            // Reset active cutscene
            activeCutscene = null;
        }

    }


    ////////////////////////////////////////////////////////////////////////
    // CHECK PLAYED ========================================================
    public bool IsCutscenePlayed(string name)
    {
        foreach (Cutscene currCutscene in cutscenes)
        {
            if (currCutscene.name == name)
            {
                return PlayerPrefs.GetInt("CutPlayed_" + currCutscene.name) == 1;
            }
        }

        return true;
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER CUTSCENE ====================================================
    // Function for use with cutscene trigger areas (triggered during level)
    public void TriggerCutscene(string name)
    {
        foreach (Cutscene currCutscene in cutscenes)
        {
            if (currCutscene.name == name && PlayerPrefs.GetInt("CutPlayed_" + currCutscene.name) != 1)
            {
                activeCutscene = currCutscene;

                PlayerPrefs.SetInt("CutPlayed_" + currCutscene.name, 1);

                StartCutscene();
                ProcessCutscenePreEffects();

                break;
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // START SKIP CUTSCENE =================================================
    public void StartSkipCutscene()
    {
        // Make sure a cutscene is playing
        if (isCurrentlyPlaying)
        {
            cutsceneAS.Pause();
            skipHold = true;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // INTERUPT SKIP CUTSCENE ==============================================
    public void InterruptSkipCutscene()
    {
        // Make sure a cutscene is playing
        if (isCurrentlyPlaying)
        {
            skipTimer = 0.0f;
            skipHold = false;

            // Resume Audio
            cutsceneAS.Play();

            // Update the skip timer UI
            if (cutsceneCanvasTimer)
            {
                cutsceneCanvasTimer.fillAmount = 0.0f;
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // GET IS PLAYING ======================================================
    public bool GetIsCurrentlyPlaying()
    {
        return isCurrentlyPlaying;
    }


    ////////////////////////////////////////////////////////////////////////
    // FORCE SET IS PLAYING ================================================
    public void ForceSetIsCurrentlyPlaying(bool isPlaying)
    {
        isCurrentlyPlaying = isPlaying;
    }


    ////////////////////////////////////////////////////////////////////////
    // DISABLE INPUT =======================================================
    public void DisableInput()
    {
        inputManager.actions.Disable();
    }


    ////////////////////////////////////////////////////////////////////////
    // RESET CUTSCENES =====================================================
    public void ResetCutscenesPlayed()
    {
        foreach (Cutscene currCutscene in cutscenes)
        {
            PlayerPrefs.SetInt("CutPlayed_" + currCutscene.name, 0);
        }
    }
}


//=================================================================================
// Cutscene Class
//=================================================================================
[Serializable]
public class Cutscene
{
    [SerializeField]
    public string name;

    [SerializeField]
    public FrameData[] frames;

    [SerializeField]
    public bool playOnSceneStart = false;

    [SerializeField]
    public bool nextSceneOnFinish = false;

    [SerializeField]
    public float timeBetween = 5.0f;

    [SerializeField]
    public UnityEvent PreEvent;

    [SerializeField]
    public UnityEvent PostEvent;

    [SerializeField]
    public AudioClip cutsceneTrack;
}


//=================================================================================
// Frame Class
//=================================================================================
[Serializable]
public class FrameData
{
    [SerializeField]
    public Sprite frameImage;
}