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

    // Transitions
    private Animator transitionCanvas;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        transitionCanvas = GameObject.FindWithTag("Transition").GetComponentInChildren<Animator>();
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

        if (isDead)
        {
            StartCoroutine(TransitionSequence());
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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
