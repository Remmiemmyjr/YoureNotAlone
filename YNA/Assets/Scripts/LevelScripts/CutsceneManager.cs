//*************************************************
// Project: We're Tethered Together
// File: CutsceneManager.cs
// Author/s: Corbyn LaMar
//
// Desc: Manages the cutscenes associated with the
//       current scene.
//
// Notes:
// - 
//
// Last Edit: 7/14/2023
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

    // Cutscene Items
    public Cutscene[] cutscenes;
    private Cutscene activeCutscene;

    private Image cutsceneCanvasFrame;
    private Image cutsceneCanvasTimer;

    private bool isCurrentlyPlaying = false;
    private int currentFrameIndex = 0;
    private float cutsceneTimer = 0.0f;

    private bool skipHold = false;
    private const float skipTheshold = 1.5f;
    private float skipTimer = 0.0f;

    private const float fadeTime = 2.0f;
    private bool inFade = false;

    private bool isStartCutscene = false;

    private PlayerInput inputManager;

    [SerializeField]
    public UnityEvent FadeToBlack;
    [SerializeField]
    public UnityEvent FadeBackIn;

    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        inputManager = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        // Get the canvas to play cutscenes on
        cutsceneCanvasFrame = transform.Find("Frame").GetComponent<Image>();
        cutsceneCanvasTimer = transform.Find("Timer").GetComponent<Image>();

        // If a cutscene is to play when the scene is loaded, then trigger it
        foreach (Cutscene currCutscene in cutscenes)
        {
            if (currCutscene.playOnSceneStart)
            {
                // Hide transition
                GameObject.FindGameObjectWithTag("Transition").GetComponentInChildren<Image>().enabled = false;

                isStartCutscene = true;

                activeCutscene = currCutscene;

                StartCutscene();
                ProcessCutsceneEffects();
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
            if (skipHold && !inFade)
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
                        //FinishCutscene();
                        //FadeBackIn.Invoke();
                        StartCoroutine(Load());
                    }
                }
            }
        }
    }


    public IEnumerator Load()
    {
        yield return new WaitForSeconds(1.5f);
        FinishCutscene();
    }

    //-------------------------------------------------------------------------------------------------
    // PRIVATE FUNCTIONS
    //-------------------------------------------------------------------------------------------------

    ////////////////////////////////////////////////////////////////////////
    // START CUTSCENE ======================================================
    public void StartCutscene()
    {
        // No player input allowed
        inputManager.actions.Disable();

        // Update sprite
        cutsceneCanvasFrame.sprite = activeCutscene.frames[currentFrameIndex].frameImage;

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
    public void ProcessCutsceneEffects()
    {

    }


    ////////////////////////////////////////////////////////////////////////
    // FINISH CUTSCENE =====================================================
    public void FinishCutscene()
    {
        // Fade back in
        FadeBackIn.Invoke();
        // Re-enable player input
        inputManager.actions.Enable();

        // Update variables
        isCurrentlyPlaying = false;

        // Ensure they exist
        if (cutsceneCanvasFrame && cutsceneCanvasTimer)
        {
            // Turn off canvas image for cutscene
            cutsceneCanvasFrame.enabled = false;
            cutsceneCanvasTimer.enabled = false;
        }

        // If marked to be at the end of a scene, advance
        if (activeCutscene.nextSceneOnFinish)
        {
            // Reset active cutscene
            activeCutscene = null;

            CheckpointController.ResetLevel();
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


    //-------------------------------------------------------------------------------------------------
    // PUBLIC FUNCTIONS
    //-------------------------------------------------------------------------------------------------

    ////////////////////////////////////////////////////////////////////////
    // TRIGGER CUTSCENE ====================================================
    public void TriggerCutscene(string name)
    {
        //--- Function for use with cutscene trigger areas ---//

        foreach (Cutscene currCutscene in cutscenes)
        {
            if (currCutscene.name == name)
            {
                activeCutscene = currCutscene;

                StartCutscene();

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
}


//=================================================================================
// Frame Class
//=================================================================================
[Serializable]
public class FrameData
{
    [SerializeField]
    public Sprite frameImage;

    [SerializeField]
    public CameraFX camFX;
}