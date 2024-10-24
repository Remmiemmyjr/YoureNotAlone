//*************************************************
// Project: We're Tethered Together
// File: EyeProfile.cs
// Author/s: Emmy Berg
//
// Desc: Control what the eye assets do during each
//       state/phase
//
// Notes:
//  -
//
// Last Edit: 5/3/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class EyeProfile : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    ActivateEyes manager;
    Animator eyeAnim;
    SpriteRenderer eyeRenderer;
    Light2D eyeLight;
    [SerializeField]
    private float eyeIntenstityOpen = 1.0f;
    [SerializeField]
    private float eyeIntenstityHalf = 0.25f;
    [SerializeField]
    private float eyeIntenstityClosed = 0.0f;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("EyeManager")?.GetComponent<ActivateEyes>();
        
        eyeAnim = GetComponent<Animator>();
        eyeRenderer = GetComponent<SpriteRenderer>();

        eyeLight = GetComponent<Light2D>();
    }


    ////////////////////////////////////////////////////////////////////////
    // STATUS SLEEPING =====================================================
    public void SetStatusSleeping()
    {
          // eyeRenderer.sprite = manager.closed;
        eyeAnim.SetTrigger("Close");
        eyeLight.intensity = eyeIntenstityClosed;
    }


    ////////////////////////////////////////////////////////////////////////
    // STATUS WAKING =======================================================
    public void SetStatusWaking()
    {
          // eyeRenderer.sprite = manager.halfway;
        eyeAnim.SetTrigger("Half");
        eyeLight.intensity = eyeIntenstityHalf;
    }


    ////////////////////////////////////////////////////////////////////////
    // STATUS ACTIVE =======================================================
    public void SetStatusActive()
    {
          // eyeRenderer.sprite = manager.open;
          // eyeAnim.Play("Eyeball_1_Open", -1, Random.Range(0.0f, 1.0f));
        eyeAnim.SetTrigger("Open");
        eyeAnim.SetFloat("CycleOffset", Random.Range(0.0f, 1.0f));

        eyeLight.intensity = eyeIntenstityOpen;
    }
}
