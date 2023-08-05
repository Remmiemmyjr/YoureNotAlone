//*************************************************
// Project: We're Tethered Together
// File: MenuManager.cs
// Author/s: Emmy Berg, Corbyn LaMar
//
// Desc: Manage the states of the main menu.
//
// Notes:
//  + Currently has other button actions. Will
//    need to move to a pause manager.
//
// Last Edit: 6/28/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public GameObject menu, options;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        options.SetActive(false);
        menu.SetActive(true);
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
        if (EventSystem.current.currentSelectedGameObject == null && (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
            || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))))
        {
            GameObject resumeButton = transform.Find("StartButton").gameObject;

            if (resumeButton)
            {
                EventSystem.current.SetSelectedGameObject(resumeButton);
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // START BUTTON ========================================================
    public void StartButton()
    {
        SceneManager.LoadScene("Tutorial1");
    }


    ////////////////////////////////////////////////////////////////////////
    // SETTINGS BUTTON =====================================================
    public void SettingsButton()
    {
        options.SetActive(true);
        menu.SetActive(false);
    }


    ////////////////////////////////////////////////////////////////////////
    // RETURN BUTTON =======================================================
    public void ReturnMenu()
    {
        options.SetActive(false);
        menu.SetActive(true);
    }


    ////////////////////////////////////////////////////////////////////////
    // QUIT BUTTON =========================================================
    public void QuitButton()
    {
        Application.Quit();
    }


    ////////////////////////////////////////////////////////////////////////
    // RESTART BUTTON ======================================================
    //public void RestartButton()
    //{
    //    SceneManager.LoadScene(Stats.currLevel);
    //}


    ////////////////////////////////////////////////////////////////////////
    // MENU BUTTON =========================================================
    public void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
