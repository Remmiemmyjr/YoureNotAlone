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
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public GameObject menu, settings, controls, confirmation;
    GameObject activeButton;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        settings.SetActive(false);
        controls.SetActive(false);
        menu.SetActive(true);
        confirmation.SetActive(false);
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
            activeButton = null;

            if (confirmation.activeSelf)
            {
                activeButton = confirmation.transform.Find("NoConfirmButton").gameObject;
            }
            else if (settings.activeSelf)
            {
                activeButton = settings.transform.Find("Menu").gameObject;
            }
            else if (controls.activeSelf)
            {
                activeButton = controls.transform.Find("Menu").gameObject;
            }
            else
            {
                activeButton = menu.transform.Find("MenuCanvasButtons").Find("StartButton").gameObject;
            }

            if (activeButton)
            {
                EventSystem.current.SetSelectedGameObject(activeButton);
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // START BUTTON ========================================================
    public void StartButton()
    {
        SceneManager.LoadScene("Tutorial-1 Revised");
    }


    ////////////////////////////////////////////////////////////////////////
    // SETTINGS BUTTON =====================================================
    public void SettingsButton()
    {
        settings.SetActive(true);
        menu.SetActive(false);

        activeButton = settings.transform.Find("Menu").gameObject;

        if (activeButton)
            EventSystem.current.SetSelectedGameObject(activeButton);
    }


    ////////////////////////////////////////////////////////////////////////
    // CONTROLS BUTTON =====================================================
    public void ControlsButton()
    {
        controls.SetActive(true);
        menu.SetActive(false);

        activeButton = controls.transform.Find("Menu").gameObject;

        if (activeButton)
            EventSystem.current.SetSelectedGameObject(activeButton);
    }


    ////////////////////////////////////////////////////////////////////////
    // RETURN BUTTON =======================================================
    public void ReturnMenu()
    {
        settings.SetActive(false);
        controls.SetActive(false);
        confirmation.SetActive(false);
        menu.SetActive(true);

        activeButton = menu.transform.Find("MenuCanvasButtons").Find("StartButton").gameObject;

        if (activeButton)
            EventSystem.current.SetSelectedGameObject(activeButton);
    }


    ////////////////////////////////////////////////////////////////////////
    // QUIT BUTTON =========================================================
    public void QuitButton()
    {
        menu.SetActive(false);
        confirmation.SetActive(true);

        activeButton = confirmation.transform.Find("NoConfirmButton").gameObject;

        if(activeButton)
            EventSystem.current.SetSelectedGameObject(activeButton);
    }

    ////////////////////////////////////////////////////////////////////////
    // CONFIRM BUTTON ======================================================
    public void ConfirmButton()
    {
        Application.Quit();
    }


    ////////////////////////////////////////////////////////////////////////
    // MENU BUTTON =========================================================
    public void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
