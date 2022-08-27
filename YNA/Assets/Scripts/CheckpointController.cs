using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//README 
//add this script to an empty gameObject, also set tag to "CC"
//No need to set a pos in the inspector, itll look for an object tagged "Respawm"
public class CheckpointController : MonoBehaviour
{

    private static CheckpointController instance;

    public Vector2 lastCheckpointPos;
    void Awake()
    {
        //look for initial Spawn
        lastCheckpointPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
