using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menu, options;

    void Start()
    {
        if(options != null)
        {
            options.SetActive(false);
        }
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Level1");
    }

    public void SettingsButton()
    {
        options.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
