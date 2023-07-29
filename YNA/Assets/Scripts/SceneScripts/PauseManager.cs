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
    private GameObject settingsCanvas;

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
        if (EventSystem.current.currentSelectedGameObject == null && Input.anyKey)
        {
            GameObject resumeButton = transform.Find("Resume").gameObject;

            if (resumeButton)
            {
                EventSystem.current.SetSelectedGameObject(resumeButton);
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // RESUME BUTTON =======================================================
    public void ResumeButton()
    {
        // Update variables
        GameObject.FindWithTag("GameManager").GetComponent<Stats>().TogglePause();

        // Reset timescale
        Time.timeScale = 1;

        // Hide Pause UI
        gameObject.SetActive(false);
    }


    ////////////////////////////////////////////////////////////////////////
    // SETTINGS BUTTON =====================================================
    public void SettingsButton()
    {
        // options.SetActive(true);
    }


    ////////////////////////////////////////////////////////////////////////
    // RETURN BUTTON =======================================================
    public void ReturnMenu()
    {
        // options.SetActive(false);
    }


    ////////////////////////////////////////////////////////////////////////
    // QUIT BUTTON =========================================================
    public void QuitButton()
    {
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
}
