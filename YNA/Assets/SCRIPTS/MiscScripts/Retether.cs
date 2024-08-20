// Project: We're Tethered Together
// File: Retether.cs
// Author/s: Corbyn LaMar
//
// Desc: Retether players at beginning of level
//
// Notes:
//  - Destroys rope in L6 
//
// Last Edit: 7/2/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retether : MonoBehaviour
{
    public static bool relink;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("RelinkCheck");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        else if (SceneManager.GetActiveScene().name != "Level-6")
        {
            Destroy(gameObject);
            relink = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Partner")
        {
            Info.grapple.startTetheredTogether = true;
            DontDestroyOnLoad(gameObject);
            relink = true;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Level-6")
        {
            Destroy(gameObject);
            relink = false;
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
