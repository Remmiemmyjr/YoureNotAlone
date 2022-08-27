using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckpointController cc;

    void Start()
    {
        cc = GameObject.FindGameObjectWithTag("CC").GetComponent<CheckpointController>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Partner"))
        {
            cc.lastCheckpointPos = transform.position;
        }
    }
}
