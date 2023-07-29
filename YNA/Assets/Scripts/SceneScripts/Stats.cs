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
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public bool isPaused = false;

    public PlayerInput playerInput;

    // Transitions
    private Animator transitionCanvas;

    GameObject pauseUI;
    private GameObject settingsCanvas;
    // *********************************************************************
    private void Awake()
    {
        transitionCanvas = GameObject.FindWithTag("Transition")?.GetComponentInChildren<Animator>();

        pauseUI = GameObject.FindWithTag("Pause");
        settingsCanvas = GameObject.FindWithTag("SettingsCanvas");

        isDead = false;
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
        if (isDead)
        {
            StartCoroutine(TransitionSequence());
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // RESET =============================================================
    public void ResetLevel(InputAction.CallbackContext ctx)
    {
        // Action performed and not in a cutscene
        if (ctx.performed && !GameObject.FindGameObjectWithTag("CutsceneCanvas").GetComponent<CutsceneManager>().GetIsCurrentlyPlaying())
        {
            GetComponent<Stats>().isDead = true;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // PAUSE =============================================================
    public void Pause(InputAction.CallbackContext ctx)
    {
        // Action performed and not in a cutscene
        if (ctx.performed && !GameObject.FindGameObjectWithTag("CutsceneCanvas").GetComponent<CutsceneManager>().GetIsCurrentlyPlaying())
        {
            // If the Pause menu isn't there, try to find it
            if(!pauseUI)
            {
                pauseUI = GameObject.FindWithTag("Pause");
            }


            // Paused -> Unpaused
            if (isPaused)
            {
                isPaused = false;

                if (pauseUI)
                {
                    // Hide Pause UI
                    pauseUI.SetActive(false);
                }

                // Reset timescale
                Time.timeScale = 1;
            }

            // Unpaused -> Paused
            else
            {
                isPaused = true;

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
                cutCanv.GetComponent<CutsceneManager>().SkipCutsceneFrame();
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // TOGGLE PAUSE ========================================================
    public void TogglePause()
    {
        isPaused = !isPaused;
    }


    ////////////////////////////////////////////////////////////////////////
    // TRANSITION ==========================================================
    private IEnumerator TransitionSequence()
    {
        StartCoroutine(GetComponent<DeathShader>().Lerp(1));
        yield return new WaitForSeconds(1.5f);

        if (transitionCanvas)
        {
            transitionCanvas.SetTrigger("EyeDeath");

            yield return new WaitForSeconds(transitionCanvas.GetCurrentAnimatorClipInfo(0).Length);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
