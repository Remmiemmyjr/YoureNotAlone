//*************************************************
// Project: We're Tethered Together
// File: ObstacleKill.cs
// Author/s: Emmy Berg
//           Corbyn LaMar
//
// Desc: Triggers particles on spike death and
//       restarts from last checkpoint
//
// Notes:
//  -
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleKill : MonoBehaviour
{
    Stats stats;

    private void Start()
    {
        stats = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Stats>();
    }

    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Partner")
        {
            StartCoroutine(stats.ObstacleDeathSequence());
        }
    }
}
