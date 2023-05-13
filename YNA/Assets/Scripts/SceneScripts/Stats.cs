//*************************************************
// Project: We're Tethered Together
// File: Stats.cs
// Author/s: Emmy Berg
//
// Desc: Manage stats for the scene
//
// Notes:
//  + See Start() and Update()'s comments
//
// Last Edit: 5/3/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    [HideInInspector]
    public bool isHidden;
    [HideInInspector]
    public bool isDead;

    [HideInInspector]
    public static string currLevel;

    Vector3 newZPos;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        // Officially set whatever this is supposed to be in the inspector/prefab
        newZPos = new Vector3(transform.position.x, transform.position.y, -1);
        transform.position = newZPos;
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        // I dont remember why I needed to do this. I think it had to do with
        // making sure "on-trigger-stay" always worked. Doesnt cost much tho?
        if (this.GetComponent<Rigidbody2D>().IsSleeping())
        {
            this.GetComponent<Rigidbody2D>().WakeUp();
        }

        if(isDead)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
