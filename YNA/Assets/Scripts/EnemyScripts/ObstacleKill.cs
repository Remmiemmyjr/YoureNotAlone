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
    static bool dontRepeat;

////////////////////////////////////////////////////////////////////////
// AWAKE ===============================================================
    void Awake()
    {
        transitionCanvas = GameObject.FindWithTag("Transition").GetComponentInChildren<Animator>();
        dontRepeat = false;
    }

////////////////////////////////////////////////////////////////////////
// TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Partner")
        {
            StartCoroutine(TransitionSequence());
        }
    }


////////////////////////////////////////////////////////////////////////
// TRANSITION SEQUENCE =================================================
    private IEnumerator TransitionSequence()
    {
        Info.isDead = true;
        Info.eyeDeath = false;

        if (!dontRepeat)
        {
            dontRepeat = true;

            if (Info.partner)
            {
                Info.partner.GetComponent<ParticleSystem>().Play();
                Info.partner.GetComponent<SpriteRenderer>().enabled = false;
            }
            Info.player.GetComponent<ParticleSystem>().Play();
            Info.player.GetComponent<SpriteRenderer>().enabled = false;
            Info.player.GetComponent<LineRenderer>().enabled = false;

            yield return new WaitForSeconds(1.5f);


            if (transitionCanvas)
            {
                transitionCanvas.SetTrigger("EyeDeath");

                yield return new WaitForSeconds(transitionCanvas.GetCurrentAnimatorClipInfo(0).Length);
            }

            // Could change back to using levelname if needed
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
