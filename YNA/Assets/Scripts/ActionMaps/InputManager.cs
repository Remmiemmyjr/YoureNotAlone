//*************************************************
// Project: We're Tethered Together
// File: InputManager.cs
// Author/s: Emmy Berg
//
// Desc: Manages the input appended to the
//       gameplay action-map.
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    [HideInInspector] public static Input input;

    GameObject player;

    PlayerController pc;
    Grapple rope;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // PROCESSED BEFORE GAMEPLAY ===========================================
    void Awake()
    {
        input = new Input();

        player = GameObject.FindGameObjectWithTag("Player");

        pc = player.GetComponent<PlayerController>();
        rope = player.GetComponent<Grapple>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
