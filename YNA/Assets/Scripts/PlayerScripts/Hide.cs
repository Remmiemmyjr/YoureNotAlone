//*************************************************
// Project: We're Tethered Together
// File: Hide.cs
// Author/s: Emmy Berg
//
// Desc: Manages the hide state for both actors.
//
// Notes:
//  + The redundancy can be greatly minimized.
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    [HideInInspector]
    public bool isHidden;

    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hideable")
        {
            isHidden = true;
            GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            
            if(gameObject.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            {
                if (gameObject.tag == "Player")
                {
                    gameObject.GetComponent<SetPlayerAnimState>().SetNextState(SetPlayerAnimState.PlayerStates.cHiding);
                }
                else
                {
                    gameObject.GetComponent<PartnerController>().state_next = PartnerController.PartnerStates.cHiding;
                }
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER EXIT ========================================================
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hideable")
        {
            isHidden = false;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
