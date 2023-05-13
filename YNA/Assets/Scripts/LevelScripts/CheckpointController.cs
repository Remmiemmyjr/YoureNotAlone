//*************************************************
// Project: We're Tethered Together
// File: CheckpointController.cs
// Author/s: Cameron Myers
//
// Desc: Set the new checkpoint position
//
// Notes:
//  + Needs modified to enable from partner bunny
//
// Last Edit: 5/3/2023
//
//*************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//README 
//add this script to an empty gameObject, also set tag to "CC"
//No need to set a pos in the inspector, itll look for an object tagged "Respawm"
public class CheckpointController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    private static bool levelRestart = true;

    public static Vector2 lastCheckpointPos;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        if(levelRestart)
        {
            //look for initial Spawn
            lastCheckpointPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        }

        levelRestart = false;
    }


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    internal static void ResetLevel()
    {
        levelRestart = true;
    }
}
