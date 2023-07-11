using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTriggerArea : MonoBehaviour
{
    [SerializeField]
    private string cutsceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (GameObject.FindGameObjectWithTag("CutsceneCanvas"))
            {
                GameObject.FindGameObjectWithTag("CutsceneCanvas").GetComponent<CutsceneManager>().TriggerCutscene(cutsceneName);
            }
        }
    }
}
