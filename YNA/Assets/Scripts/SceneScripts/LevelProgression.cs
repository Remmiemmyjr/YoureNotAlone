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
    GameObject displayMessage;

    [SerializeField]
    private string nextLevel;

    public bool requiresPartner;

    // Transitions
    private Animator transitionCanvas;
    // *********************************************************************

    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        displayMessage = GameObject.FindWithTag("ProceedCanvas")?.transform.GetChild(0).gameObject;
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        instructions.SetActive(false);
        displayMessage?.SetActive(false);

        transitionCanvas = GameObject.FindWithTag("Transition")?.GetComponentInChildren<Animator>();

        PlayerPrefs.SetInt("currentLevelBuildIndex", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!requiresPartner || (collision.GetComponent<Grapple>().isTethered))
            {
                StartCoroutine(TransitionSequence());
            }
            else
            {
                displayMessage?.SetActive(true);
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER EXIT =======================================================
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            displayMessage?.SetActive(false);
        }
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

        SceneManager.LoadScene(nextLevel);
    }
}
