//*************************************************
// Project: We're Tethered Together
// File: CheckpointController.cs
// Author/s: Cameron Myers
//
// Desc: Set the new checkpoint position
//
// Notes:
//  -
//
// Last Edit: 5/3/2023
//
//*************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// README *************************************************************************
// add this script to an empty gameObject, also set tag to "CC"
// No need to set a pos in the inspector, itll look for an object tagged "Respawn"
// ********************************************************************************
public class CheckpointController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public static Vector2 lastCheckpointPos;

    GameObject player, partner;
    Vector3 checkpointPos;
    public bool startWithPartner = true;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // ON DESTROY ==========================================================
    // When this is destroyed, store what the "previous scene" was to avoid
    // cutscene repeats
    public static string PreviousLevel { get; private set; }
    // When this is destroyed, reset all cutscene play values
    private void OnDestroy()
    {
        PreviousLevel = gameObject.scene.name;
    }


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        partner = GameObject.FindGameObjectWithTag("Partner");

        if (PreviousLevel != gameObject.scene.name)
        {
            //look for initial Spawn
            ResetCheckpoints();
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        checkpointPos = lastCheckpointPos;
        SetPos(player, new Vector3(0, 0, 0));

        // Make sure there is a partner in this level
        if (partner && startWithPartner)
            SetPos(partner, new Vector3(-0.5f, 0, 0));
    }


    ////////////////////////////////////////////////////////////////////////
    // SET POS =============================================================
    public void SetPos(GameObject entity, Vector3 offset)
    {
        entity.transform.position = checkpointPos + offset;
    }


    ////////////////////////////////////////////////////////////////////////
    // RESET CHECKPOINT ====================================================
    public void ResetCheckpoints()
    {
        lastCheckpointPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }

}
