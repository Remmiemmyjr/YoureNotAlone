//*************************************************
// Project: We're Tethered Together
// File: StoneProfile.cs
// Author/s: Emmy Berg
//
// Desc: Manager properties for the stone object
//       overlay
//
// Notes:
// -
//
// Last Edit: 8/05/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneProfile : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    SpriteRenderer spr;
    GameObject owner;
    Animator ownerAnim;
    bool shouldDoThis;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        shouldDoThis = true;
        spr = GetComponent<SpriteRenderer>();
        owner = transform.parent.gameObject;
        ownerAnim = owner.GetComponent<Animator>();
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        // y no workie
        spr.flipX = owner.GetComponent<SpriteRenderer>().flipX;

        if (Info.isDead)
        {
            owner.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            // Dont do this again/repeatedly after already triggering
            if (shouldDoThis)
            {
                shouldDoThis = false;

                if (owner.tag == "Player")
                    ownerAnim.Play("Player_Still");

                else if (owner.tag == "Partner")
                    ownerAnim.Play("Partner_Still");
            }
        }
    }
}
