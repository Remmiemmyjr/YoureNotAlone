//*************************************************
// Project: We're Tethered Together
// File: MenuManager.cs
// Author/s: Emmy Berg
//
// Desc: Manage the states of the main menu.
//
// Notes:
//  + Currently has other button actions. Will
//    need to move to a pause manager.
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    // START BUTTON ========================================================
    public void StartButton()
    {
        SceneManager.LoadScene("Level1");
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
