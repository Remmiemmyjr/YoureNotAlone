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

public class LevelProgression : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    GameObject Player;

    public GameObject instructions;
    public GameObject displayMessage;

    public string nextLevel;

    public bool requiresPartner;
    bool atExit = false;

    [SerializeField]
    private bool proceedOnTouch = false;

    // Transitions
    private Animator transitionCanvas;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        instructions.SetActive(false);
        displayMessage?.SetActive(false);

        transitionCanvas = GameObject.FindWithTag("Transition")?.GetComponentInChildren<Animator>();
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (atExit)
            {
                if (requiresPartner == true)
                {
                    if (Player.GetComponent<Grapple>().isTethered == false)
                    {
                        StartCoroutine(DisplayMessage());
                    }
                    else
                    {
                        StartCoroutine(TransitionSequence());
                    }
                }
                else
                {
                    StartCoroutine(TransitionSequence());
                }
            }
        }

    }


    ////////////////////////////////////////////////////////////////////////
    // DISPLAY MESSAGE =====================================================
    IEnumerator DisplayMessage()
    {
        Debug.Log("You cannot proceed without your partner");
        displayMessage?.SetActive(true);
        yield return new WaitForSeconds(2f);
        displayMessage?.SetActive(false);
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(proceedOnTouch)
        {
            StartCoroutine(TransitionSequence());
        }
        else
        {
            instructions.SetActive(true);
            atExit = true;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER EXIT ========================================================
    private void OnTriggerExit2D(Collider2D collision)
    {
        instructions.SetActive(false);
        atExit = false;
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
