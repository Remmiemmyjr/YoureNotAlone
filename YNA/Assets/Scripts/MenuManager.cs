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
        SceneManager.LoadScene("Level1");
    }

    public void SettingsButton()
    {
        options.SetActive(true);
        menu.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
