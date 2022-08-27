using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{

    private static CheckpointController instance;

    public Vector2 lastCheckpointPos;
    void Awake()
    {

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
