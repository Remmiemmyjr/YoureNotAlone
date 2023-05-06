//*************************************************
// Project: We're Tethered Together
// File: ActivateEyes.cs
// Author/s: Emmy Berg
//           K Preston
//
// Desc: Controls the Eyes three states
//
// Notes:
//  + Holy shit we need more helper functions lol
//
// Last Edit: 5/3/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EyeProfile : MonoBehaviour
{
    ActivateEyes manager;
    Animator eyeAnim;
    SpriteRenderer eyeRenderer;

    public Sprite closed;
    public Sprite halfway;
    public Sprite open;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("EyeManager").GetComponent<ActivateEyes>();

        eyeAnim = GetComponent<Animator>();
        eyeRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetStatusSleeping()
    {
        eyeRenderer.sprite = closed;
        eyeAnim.Play("Closed");
        eyeAnim.SetBool("isOpen", manager.timeToHide);
    }

    public void SetStatusWaking()
    {
        eyeRenderer.sprite = halfway;
        eyeAnim.Play("EyeballHalfway");
    }

    public void SetStatusActive()
    {
        eyeRenderer.sprite = open;
        eyeAnim.Play("EyeballOpen");
        eyeAnim.SetBool("isOpen", manager.timeToHide);
    }
}
