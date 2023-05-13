//*************************************************
// Project: We're Tethered Together
// File: ObstacleKill.cs
// Author/s: Emmy Berg
//
// Desc: Restarts level on death
//
// Notes:
//  + Add death effects / better transition
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleKill : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //could change back to using levelname if needed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
