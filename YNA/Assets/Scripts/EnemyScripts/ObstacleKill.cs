//*************************************************
// Project: We're Tethered Together
// File: ObstacleKill.cs
// Author/s: Emmy Berg
//           Corbyn LaMar
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
    // Transitions
    private Animator transitionCanvas;

////////////////////////////////////////////////////////////////////////
// AWAKE ===============================================================
    void Awake()
    {
        transitionCanvas = GameObject.FindWithTag("Transition").GetComponentInChildren<Animator>();
    }

////////////////////////////////////////////////////////////////////////
// TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(TransitionSequence());
    }


////////////////////////////////////////////////////////////////////////
// TRANSITION SEQUENCE =================================================
    private IEnumerator TransitionSequence()
    {
        if (transitionCanvas)
        {
            transitionCanvas.SetTrigger("EyeDeath");

            yield return new WaitForSeconds(transitionCanvas.GetCurrentAnimatorClipInfo(0).Length);
        }

        // Could change back to using levelname if needed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
