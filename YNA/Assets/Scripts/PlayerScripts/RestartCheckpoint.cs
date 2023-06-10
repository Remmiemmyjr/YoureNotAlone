using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartCheckpoint : MonoBehaviour
{
    GameObject player, partner;
    Vector3 checkpointPos;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        partner = GameObject.FindGameObjectWithTag("Partner");
    }

    void Start()
    {
        checkpointPos = CheckpointController.lastCheckpointPos;
        SetPos(player, new Vector3(0, 0, 0));
        SetPos(partner, new Vector3(-0.5f, 0, 0));
    }

    public void SetPos(GameObject entity, Vector3 offset)
    {
        entity.transform.position = checkpointPos + offset;
    }
}
