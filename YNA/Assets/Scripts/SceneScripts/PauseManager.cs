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
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES FOR FANCY STUFF ===========================================
    private Animator transitionCanvas;
    private GameObject pauseCanvas;
    private GameObject settingsCanvas;
    private GameObject confirmationCanvas;

    private bool inSettings = false;
    private bool inConfirmation = false;

    // Eye Manager for pause fx
    GameObject eyeManager;

    // Music controller for pause fx
    GameObject musicController;

    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        pauseCanvas = GameObject.FindWithTag("Pause");
        settingsCanvas = GameObject.FindWithTag("SettingsCanvas");
        confirmationCanvas = GameObject.FindWithTag("ConfirmationCanvas");
        eyeManager = GameObject.FindWithTag("EyeManager");
        musicController = GameObject.FindWithTag("MusicController");

        settingsCanvas.SetActive(false);
        confirmationCanvas.SetActive(false);
    }

    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        transitionCanvas = GameObject.FindWithTag("Transition").GetComponentInChildren<Animator>();
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
        if (EventSystem.current.currentSelectedGameObject == null && Input.anyKey && !inSettings)
        {
            GameObject activeButton = null;

            if (inSettings)
            {
                activeButton = settingsCanvas.transform.Find("Menu").gameObject;
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
        if (!inSettings && !inConfirmation)
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
        if (!inSettings && !inConfirmation)
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
    // RETURN BUTTON =======================================================
    public void ReturnMenu()
    {
        if (inSettings)
        {
            settingsCanvas.SetActive(false);
            inSettings = false;
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
        if (!inSettings && !inConfirmation)
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

    public bool GetInSubMenu()
    {
        return (inSettings || inConfirmation);
    }
}
