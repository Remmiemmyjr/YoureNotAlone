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
    private bool skipFrame = false;

    private bool isStartCutscene = false;

    private PlayerInput inputManager;

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
            // Update cutscene time
            cutsceneTimer += Time.deltaTime;

            // Check if the timer has elapsed
            if (cutsceneTimer >= activeCutscene.timeBetween || skipFrame)
            {
                // Reset timer and possible frame skip
                cutsceneTimer = 0.0f;
                skipFrame = false;

                // Advance or end cutscene
                if (currentFrameIndex < activeCutscene.frames.Length - 1)
                {
                    currentFrameIndex++;

                    cutsceneCanvasFrame.sprite = activeCutscene.frames[currentFrameIndex].frameImage;

                    ProcessCutsceneEffects();
                }
                else
                {
                    FinishCutscene();
                }
            }
            else
            {
                // Update the timer UI
                if (cutsceneCanvasTimer)
                {
                    cutsceneCanvasTimer.fillAmount = 1.0f - (cutsceneTimer / activeCutscene.timeBetween);
                }
            }
        }
    }


    //-------------------------------------------------------------------------------------------------
    // PRIVATE FUNCTIONS
    //-------------------------------------------------------------------------------------------------

    ////////////////////////////////////////////////////////////////////////
    // START CUTSCENE ======================================================
    private void StartCutscene()
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
    private void ProcessCutsceneEffects()
    {

    }


    ////////////////////////////////////////////////////////////////////////
    // FINISH CUTSCENE =====================================================
    private void FinishCutscene()
    {
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
    // SKIP CUTSCENE FRAME =================================================
    public void SkipCutsceneFrame()
    {
        // Make sure a cutscene is playing
        if (isCurrentlyPlaying)
        {
            skipFrame = true;
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