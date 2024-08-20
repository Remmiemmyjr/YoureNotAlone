//*************************************************
// Project: We're Tethered Together
// File: DisableEyes.cs
// Author/s: Emmy Berg
//
// Desc: Manage eye deactivation
//
// Notes:
//  - 
//
// Last Edit: 7/2/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class DisableEyes : MonoBehaviour
{
    public ActivateEyes eyeManager;
    public GameObject eyes;

    [SerializeField]
    private bool enableEyes = false;
    [SerializeField]
    private bool camShake = false;
    [SerializeField]
    private bool killEyes = false;

    [SerializeField]
    private string targetTag = "Player";
    
    CinemachineImpulseSource impulse;


    private void Awake()
    {
        impulse = GetComponent<CinemachineImpulseSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == targetTag)
        {
            eyeManager.enabled = enableEyes;

            if (enableEyes == true)
            {
                eyeManager.currTime = eyeManager.wakingTime + 0.01f;
                eyeManager.seekTime = 4;
                eyeManager.prevStatus = EyeStates.SLEEPING;
                eyeManager.status = EyeStates.WAKING;
            }

            else if (enableEyes == false)
            {
                eyeManager.currTime = 20f;
                eyeManager.seekTime = -1;
                eyeManager.prevStatus = EyeStates.ACTIVE;
                eyeManager.status = EyeStates.SLEEPING;
                eyeManager.Sleep();
            }

            if (killEyes && eyes)
                eyes.SetActive(false);

            if (camShake && impulse)
                CameraShake.manager.Shake(impulse, 0.35f);
        }
    }

    private void Update()
    {
        //if (eyeManager.status == EyeStates.SEEN)
        //{
        //    //Info.isDead = true;
        //    //Info.eyeDeath = true;
        //    eyeManager.timeInSight = 0.1f;
        //}
    }
}
