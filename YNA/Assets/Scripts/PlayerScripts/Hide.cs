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
    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Partner")
        {
            collision.gameObject.GetComponent<Stats>().isHidden = true;
            collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER EXIT ========================================================
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Partner")
        {
            collision.gameObject.GetComponent<Stats>().isHidden = false;
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
