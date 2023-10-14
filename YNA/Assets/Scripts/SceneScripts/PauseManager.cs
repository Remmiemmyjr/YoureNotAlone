//*************************************************
// Project: We're Tethered Together
// File: PauseManager.cs
// Author/s: Corbyn LaMar
//
// Desc: Manage the states of the pause menu.
//
// Notes:
//
// Last Edit: 6/28/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PauseManager : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES FOR FANCY STUFF ===========================================
    private Animator transitionCanvas;
    private GameObject pauseCanvas;
    private GameObject settingsCanvas;
    private GameObject controlsCanvas;
    private GameObject confirmationCanvas;

    private GameObject cutsceneCanvas;
    private GameObject checkpointController;

    private bool inSettings = false;
    private bool inControls = false;
    private bool inConfirmation = false;

    // Eye Manager for pause fx
    GameObject eyeManager;

    // Music controller for pause fx
    GameObject musicController;

    [SerializeField]
    private AudioClip[] paperSounds;

    private AudioSource sfxManagerUI;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        pauseCanvas = GameObject.FindWithTag("Pause");
        settingsCanvas = GameObject.FindWithTag("SettingsCanvas");
        controlsCanvas = GameObject.FindWithTag("ControlsCanvas");
        confirmationCanvas = GameObject.FindWithTag("ConfirmationCanvas");
        eyeManager = GameObject.FindWithTag("EyeManager");
        musicController = GameObject.FindWithTag("MusicController");

        cutsceneCanvas = GameObject.FindWithTag("CutsceneCanvas");
        checkpointController = GameObject.FindWithTag("CC");

        controlsCanvas.SetActive(false);
        confirmationCanvas.SetActive(false);

        sfxManagerUI = GetComponent<AudioSource>();
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        transitionCanvas = GameObject.FindWithTag("Transition").GetComponentInChildren<Animator>();

        // Add input callback
        InputSystem.onDeviceChange += InputConnectionChangeCallback;
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        // If the player is using the mouse, disable highlights from button selection
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        // If the player wants to use buttons after using the mouse, reset the active object
        if (EventSystem.current.currentSelectedGameObject == null && (Input.anyKey || Mathf.Abs(Input.GetAxis("Vertical")) > 0 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0))
        {
            GameObject activeButton = null;

            if (inSettings)
            {
                activeButton = settingsCanvas.transform.Find("Menu").gameObject;
            }
            else if (inControls)
            {
                activeButton = controlsCanvas.transform.Find("Menu").gameObject;
            }
            else if (inConfirmation)
            {
                activeButton = confirmationCanvas.transform.Find("NoConfirmButton").gameObject;
            }
            else
            {
                activeButton = pauseCanvas.transform.Find("Resume").gameObject;
            }

            if (activeButton)
            {
                EventSystem.current.SetSelectedGameObject(activeButton);
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // RESUME BUTTON =======================================================
    public void ResumeButton()
    {
        if (!inSettings && !inControls && !inConfirmation)
        {
            // Update variables
            GameObject.FindWithTag("GameManager").GetComponent<Stats>().TogglePause();

            if (eyeManager)
            {
                eyeManager.GetComponent<ActivateEyes>().PauseEyeAudio(false);
            }

            if (musicController)
            {
                musicController.GetComponent<PersistantMusic>().ApplyPauseEffects(false);
            }

            // Reset timescale
            Time.timeScale = 1;

            // Hide Pause UI
            pauseCanvas.SetActive(false);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // SETTINGS BUTTON =====================================================
    public void SettingsButton()
    {
        if (!inSettings && !inControls && !inConfirmation)
        {
            settingsCanvas.SetActive(true);
            inSettings = true;

            GameObject activeButton = settingsCanvas.transform.Find("Menu").gameObject;

            if (activeButton)
            {
                EventSystem.current.SetSelectedGameObject(activeButton);
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // CONTROLS BUTTON =====================================================
    public void ControlsButton()
    {
        if (!inSettings && !inControls && !inConfirmation)
        {
            controlsCanvas.SetActive(true);
            inControls = true;

            GameObject activeButton = controlsCanvas.transform.Find("Menu").gameObject;

            if (activeButton)
            {
                EventSystem.current.SetSelectedGameObject(activeButton);
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // RETURN BUTTON =======================================================
    public void ReturnMenu()
    {
        if (inSettings)
        {
            settingsCanvas.SetActive(false);
            inSettings = false;
        }
        if (inControls)
        {
            controlsCanvas.SetActive(false);
            inControls = false;
        }
        if (inConfirmation)
        {
            confirmationCanvas.SetActive(false);
            inConfirmation = false;
        }

        pauseCanvas.SetActive(true);

        GameObject activeButton = pauseCanvas.transform.Find("Resume").gameObject;

        if (activeButton)
        {
            EventSystem.current.SetSelectedGameObject(activeButton);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // QUIT BUTTON =========================================================
    public void QuitButton()
    {
        if (!inSettings && !inControls && !inConfirmation)
        {
            pauseCanvas.SetActive(false);
            confirmationCanvas.SetActive(true);
            inConfirmation = true;

            GameObject activeButton = confirmationCanvas.transform.Find("NoConfirmButton").gameObject;

            if (activeButton)
            {
                EventSystem.current.SetSelectedGameObject(activeButton);
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // CONFIRM BUTTON ======================================================
    public void ConfirmButton()
    {
        // Show Scene
        confirmationCanvas.SetActive(false);

        // Reset timescale
        Time.timeScale = 1;

        // Reset Level Data
        cutsceneCanvas.GetComponent<CutsceneManager>().ResetCutscenesPlayed();
        checkpointController.GetComponent<CheckpointController>().ResetCheckpoints();

        // Do the animation
        StartCoroutine(TransitionSequence());
    }


    ////////////////////////////////////////////////////////////////////////
    // TRANSITION ON QUIT ==================================================
    private IEnumerator TransitionSequence()
    {
        if (transitionCanvas)
        {
            transitionCanvas.SetTrigger("EyeDeath");

            yield return new WaitForSeconds(transitionCanvas.GetCurrentAnimatorClipInfo(0).Length);
        }

        SceneManager.LoadScene("MainMenu");
    }


    ////////////////////////////////////////////////////////////////////////
    // GET IN SUB MENU =====================================================
    public bool GetInSubMenu()
    {
        return (inSettings || inConfirmation || inControls);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!Info.isPaused && !hasFocus && !GameObject.FindGameObjectWithTag("CutsceneCanvas").GetComponent<CutsceneManager>().GetIsCurrentlyPlaying())
        {
            Info.isPaused = true;

            if (pauseCanvas)
            {
                // Show Pause UI
                pauseCanvas.SetActive(true);

                // Set first button as being active
                GameObject resumeButton = pauseCanvas.transform.Find("Resume").gameObject;

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

    private void InputConnectionChangeCallback(object obj, InputDeviceChange change)
    {
        bool controllerDC = false;
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                // Device got unplugged.
                controllerDC = true;
                break;
            case InputDeviceChange.Removed:
                // Remove from Input System entirely; by default, Devices stay in the system once discovered.
                controllerDC = true;
                break;
            default:
                // See InputDeviceChange reference for other event types.
                break;
        }

        // Pause on controller DC
        if (controllerDC && !Info.isPaused && !GameObject.FindGameObjectWithTag("CutsceneCanvas").GetComponent<CutsceneManager>().GetIsCurrentlyPlaying())
        {
            Info.isPaused = true;

            if (pauseCanvas)
            {
                // Show Pause UI
                pauseCanvas.SetActive(true);

                // Set first button as being active
                GameObject resumeButton = pauseCanvas.transform.Find("Resume").gameObject;

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

    public void PlayPaperSound()
    {
        sfxManagerUI.clip = paperSounds[Random.Range(0, paperSounds.Length)];
        sfxManagerUI.Play();
    }
}
