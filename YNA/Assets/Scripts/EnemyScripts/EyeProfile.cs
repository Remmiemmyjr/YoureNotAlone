//*************************************************
// Project: We're Tethered Together
// File: ActivateEyes.cs
// Author/s: Emmy Berg
//           K Preston
//
// Desc: Controls the Eyes three states
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

    void Awake()
    {

        manager = GameObject.FindGameObjectWithTag("EyeManager")?.GetComponent<ActivateEyes>();
        
        eyeAnim = GetComponent<Animator>();
        eyeRenderer = GetComponent<SpriteRenderer>();

        eyeLight = GetComponent<Light2D>();
    }

    public void SetStatusSleeping()
    {
        eyeRenderer.sprite = manager.closed;
        eyeAnim.Play("EyeballClosed");
        eyeAnim.SetBool("isOpen", manager.timeToHide);
        eyeLight.intensity = eyeIntenstityClosed;
    }

    public void SetStatusWaking()
    {
        eyeRenderer.sprite = manager.halfway;
        eyeAnim.Play("EyeballHalfway");
        eyeLight.intensity = eyeIntenstityHalf;
    }

    public void SetStatusActive()
    {
        eyeRenderer.sprite = manager.open;
        eyeAnim.Play("EyeballOpen", -1, Random.Range(0.0f, 1.0f));
        eyeAnim.SetBool("isOpen", manager.timeToHide);
        eyeLight.intensity = eyeIntenstityOpen;
    }
}
