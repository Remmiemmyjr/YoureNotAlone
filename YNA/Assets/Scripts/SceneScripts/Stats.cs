//*************************************************
// Project: We're Tethered Together
// File: Stats.cs
// Author/s: Emmy Berg
//           Corbyn Lamar
//
// Desc: Manage stats for the scene
//
// Notes:
// - 
//
// Last Edit: 7/2/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Stats : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    //[HideInInspector]
    //public bool isDead;
    //public bool isPaused = false;

    [SerializeField]
    private bool isMainMenu = false;

    public PlayerInput playerInput;
    DeathShader stoneShader;

    // Transitions
    private Animator transitionCanvas;

    GameObject pauseUI;
    GameObject pauseMan;
    private GameObject settingsCanvas;

    // Eye Manager for pause fx
    GameObject eyeManager;

    // Music controller for pause fx
    GameObject musicController;

    // *********************************************************************
    private void Awake()
    {
        transitionCanvas = GameObject.FindWithTag("Transition")?.GetComponentInChildren<Animator>();

        pauseUI = GameObject.FindWithTag("Pause");
        pauseMan = GameObject.FindWithTag("PauseManager");
        settingsCanvas = GameObject.FindWithTag("SettingsCanvas");
        eyeManager = GameObject.FindWithTag("EyeManager");
        musicController = GameObject.FindWithTag("MusicController");
        stoneShader = gameObject.GetComponent<DeathShader>();

        Info.isDead = false;
        Info.isPaused = false;
    }

    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        if (pauseUI)
        {
            // Hide Pause UI
            pauseUI.SetActive(false);
        }

        if (settingsCanvas)
        {
            // Hide Pause UI
            settingsCanvas.SetActive(false);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        if (Info.isDead)
        {
            StartCoroutine(TransitionSequence());
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // RESET =============================================================
    public void ResetLevel(InputAction.CallbackContext ctx)
    {
        // Action performed and not in a cutscene
        if (!isMainMenu && ctx.performed && !Info.isDead && 
            !GameObject.FindGameObjectWithTag("CutsceneCanvas").GetComponent<CutsceneManager>().GetIsCurrentlyPlaying())
        {
            Info.isDead = true;
            Info.eyeDeath = true;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // PAUSE =============================================================
    public void Pause(InputAction.CallbackContext ctx)
    {
        // Action performed and not in a cutscene
        if (ctx.performed && !isMainMenu && !Info.isDead && !GameObject.FindGameObjectWithTag("CutsceneCanvas").GetComponent<CutsceneManager>().GetIsCurrentlyPlaying())
        {
            // Paused -> Unpaused
            if (Info.isPaused && !pauseMan.GetComponent<PauseManager>().GetInSubMenu())
            {
                Info.isPaused = false;

                if (pauseUI)
                {
                    // Hide Pause UI
                    pauseUI.SetActive(false);
                }

                if(eyeManager)
                {
                    eyeManager.GetComponent<ActivateEyes>().PauseEyeAudio(false);
                }

                if(musicController)
                {
                    musicController.GetComponent<PersistantMusic>().ApplyPauseEffects(false);
                }

                // Reset timescale
                Time.timeScale = 1;
            }

            // Unpaused -> Paused
            else if (!Info.isPaused)
            {
                Info.isPaused = true;

                if (pauseUI)
                {
                    // Show Pause UI
                    pauseUI.SetActive(true);

                    // Set first button as being active
                    GameObject resumeButton = pauseUI.transform.Find("Resume").gameObject;

                    if (resumeButton)
                    {
                        EventSystem.current.SetSelectedGameObject(resumeButton);
                    }
                }

                if (eyeManager)
                {
                    eyeManager.GetComponent<ActivateEyes>().PauseEyeAudio(true);
                }

                if (musicController)
                {
                    musicController.GetComponent<PersistantMusic>().ApplyPauseEffects(true);
                }

                // Set paused timescale
                Time.timeScale = 0;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // PROGRESS ============================================================
    public void Progress(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            GameObject cutCanv = GameObject.FindGameObjectWithTag("CutsceneCanvas");

            if (cutCanv)
            {
                cutCanv.GetComponent<CutsceneManager>().StartSkipCutscene();
            }
        }

        if (ctx.canceled)
        {
            GameObject cutCanv = GameObject.FindGameObjectWithTag("CutsceneCanvas");

            if (cutCanv)
            {
                cutCanv.GetComponent<CutsceneManager>().InterruptSkipCutscene();
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TOGGLE PAUSE ========================================================
    public void TogglePause()
    {
        Info.isPaused = !Info.isPaused;
    }
    

    ////////////////////////////////////////////////////////////////////////
    // TRANSITION ==========================================================
    private IEnumerator TransitionSequence()
    {
        if (stoneShader && Info.eyeDeath)
        {
            StartCoroutine(stoneShader.Lerp(1));
        }

        yield return new WaitForSeconds(1.5f);

        if (transitionCanvas)
        {
            transitionCanvas.SetTrigger("EyeDeath");

            yield return new WaitForSeconds(transitionCanvas.GetCurrentAnimatorClipInfo(0).Length);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
