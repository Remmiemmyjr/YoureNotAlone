//*************************************************
// Project: We're Tethered Together
// File: CutsceneTriggerArea.cs
// Author/s: Corbyn LaMar
//           Emmy Berg
//
// Desc: Play cutscene when player walks through
//       this trigger area
//
// Last Edit: 8/11/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneTriggerArea : MonoBehaviour
{
    [SerializeField]
    private string cutsceneName;

    [SerializeField]
    private bool requiresPartner;

    GameObject displayMessage;

    CutsceneManager csm;

    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    private void Awake()
    {
        displayMessage = GameObject.FindWithTag("ProceedCanvas")?.transform.GetChild(0).gameObject;
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!requiresPartner || (collision.GetComponent<Grapple>().isTethered))
            {
                if (GameObject.FindGameObjectWithTag("CutsceneCanvas"))
                {
                    csm = GameObject.FindGameObjectWithTag("CutsceneCanvas").GetComponent<CutsceneManager>();

                    if (!csm.IsCutscenePlayed(cutsceneName))
                    {
                        csm.ForceSetIsCurrentlyPlaying(true);
                        StartCoroutine(CutsceneLoad());
                    }
                }
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
    // LOAD ================================================================
    public IEnumerator CutsceneLoad()
    {
        csm.DisableInput();
        yield return new WaitForSeconds(0.55f);
        csm.FadeToBlack.Invoke();
        yield return new WaitForSeconds(0.75f);
        csm.FadeBackIn.Invoke();
        csm.TriggerCutscene(cutsceneName);
    }
}
