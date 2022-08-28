using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//README 
//add this script to an empty gameObject, also set tag to "CC"
//No need to set a pos in the inspector, itll look for an object tagged "Respawm"
public class CheckpointController : MonoBehaviour
{
    private static bool levelRestart = true;

    private static CheckpointController instance;

    public static Vector2 lastCheckpointPos;

    void Awake()
    {
        if(levelRestart)
        {
            //look for initial Spawn
            lastCheckpointPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        }

        levelRestart = false;
        
        //lastCheckpointPos.x -= 1;

        instance = this;


        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(instance);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    internal static void ResetLevel()
    {
        levelRestart = true;
    }
}
