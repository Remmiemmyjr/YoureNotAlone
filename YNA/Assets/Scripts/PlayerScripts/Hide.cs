//*************************************************
// Project: We're Tethered Together
// File: Hide.cs
// Author/s: Emmy Berg
//
// Desc: Manages the hide state for both actors.
//
// Notes:
// - 
//
// Last Edit: 7/05/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Hide : MonoBehaviour
{
    [HideInInspector]
    public bool isHidden;
    bool isPlayer;

    public Light2D myLight;
    float power;

    private void Start()
    {
        if (gameObject.tag == "Player")
        {
            isPlayer = true;
            myLight.enabled = false;
        }
        else
        {
            isPlayer = false;
            power = myLight.intensity;
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hideable")
        {
            isHidden = true;

            GetComponent<SpriteRenderer>().color = new Color(0.45f, 0.45f, 0.45f);

            if (isPlayer)
                myLight.enabled = true;
            else
                myLight.intensity = power - 0.5f;
            
            if(gameObject.GetComponent<Rigidbody2D>()?.velocity == Vector2.zero)
            {
                if (isPlayer)
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
            if (isPlayer)
                myLight.enabled = false;
            else
                myLight.intensity = power;
        }
    }
}
