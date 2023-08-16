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

    CutsceneManager csm;


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (GameObject.FindGameObjectWithTag("CutsceneCanvas"))
            {
                csm = GameObject.FindGameObjectWithTag("CutsceneCanvas").GetComponent<CutsceneManager>();
                StartCoroutine(Load());
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // LOAD ================================================================
    public IEnumerator Load()
    {
        csm.DisableInput();
        yield return new WaitForSeconds(0.55f);
        csm.FadeToBlack.Invoke();
        yield return new WaitForSeconds(0.75f);
        csm.FadeBackIn.Invoke();
        csm.TriggerCutscene(cutsceneName);
    }
}
