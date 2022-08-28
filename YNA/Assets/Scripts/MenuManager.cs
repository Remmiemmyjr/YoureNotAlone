using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menu, options;

    void Start()
    {
        options.SetActive(false);
        menu.SetActive(true);
    }

    public void StartButton()
    {
        SceneManager.LoadScene("LevelOne");
    }

    public void SettingsButton()
    {
        options.SetActive(true);
        menu.SetActive(false);
    }

    public void ReturnMenu()
    {
        options.SetActive(false);
        menu.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(Stats.currLevel);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
