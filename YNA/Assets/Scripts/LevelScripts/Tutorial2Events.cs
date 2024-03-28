using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial2Events : MonoBehaviour
{
    [SerializeField]
    private ActivateEyes eyeMan;

    [SerializeField]
    private string cutsceneName;

    private void Start()
    {
        // Specific case for T2, if cutscene played and death, start eyes on start
        if(PlayerPrefs.GetInt("CutPlayed_" + cutsceneName) != 0)
        {
            eyeMan.enabled = true;
        }
        else
        {
            eyeMan.enabled = false;
        }
    }

    public void ActivateEyeSequecence()
    {
        eyeMan.enabled = true;
    }
}
