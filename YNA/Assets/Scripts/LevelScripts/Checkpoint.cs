using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//README
//Create a sprite/gameobject and add this script, tag it with "Spawn"
//for every checkpoint after, repeat but tag with "Checkpoint"
//NOTE: the checkpoint tag doesnt actually do anything rn
public class Checkpoint : MonoBehaviour
{
    //private CheckpointController cc;
    public Sprite CheckOn;
    void Start()
    {
        //grab the controller
        //cc = GameObject.FindGameObjectWithTag("CC").GetComponent<CheckpointController>();

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //check for checkpoint reached
        if(other.CompareTag("Player"))
        {
            CheckpointController.lastCheckpointPos = transform.position;

            GetComponent<SpriteRenderer>().sprite = CheckOn;
        }
    }
}
