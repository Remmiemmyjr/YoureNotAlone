//*************************************************
// Project: We're Tethered Together
// File: LevelProgression.cs
// Author/s: Emmy Berg
//
// Desc: Sets next level when entering door
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelProgression : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public GameObject instructions;
    public GameObject displayMessage;

    public string nextLevel;

    public bool requiresPartner;

    // Transitions
    private Animator transitionCanvas;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        instructions.SetActive(false);
        displayMessage?.SetActive(false);

        transitionCanvas = GameObject.FindWithTag("Transition")?.GetComponentInChildren<Animator>();
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(TransitionSequence());
    }


    ////////////////////////////////////////////////////////////////////////
    // TRANSITION ==========================================================
    private IEnumerator TransitionSequence()
    {
        if (transitionCanvas)
        {
            transitionCanvas.SetTrigger("EyeDeath");

            yield return new WaitForSeconds(transitionCanvas.GetCurrentAnimatorClipInfo(0).Length);
        }

        CheckpointController.ResetLevel();
        SceneManager.LoadScene(nextLevel);
    }
}
