//*************************************************
// Project: We're Tethered Together
// File: MenuDust.cs
// Author/s: Corbyn LaMar
//
// Desc: Make a little poof for menu text falling
//
// Notes:
// -
//
// Last Edit: 8/1/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuDust : MonoBehaviour
{
    private ParticleSystem particleSys;
    private bool isUsed = false;

    void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "MenuFloor" && !isUsed)
        {
            particleSys.Play();
            isUsed = true;
        }
    }
}
