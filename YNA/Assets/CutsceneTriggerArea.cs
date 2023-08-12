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

    public IEnumerator Load()
    {
        yield return new WaitForSeconds(0.5f);
        csm.FadeToBlack.Invoke();
        yield return new WaitForSeconds(0.75f);
        csm.FadeBackIn.Invoke();
        csm.TriggerCutscene(cutsceneName);
    }
}
