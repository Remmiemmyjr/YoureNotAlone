using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleKill : MonoBehaviour
{
    private CheckpointController cc;
    private GameObject Player;
    // private GameObject Partner;

    // public string levelName;

    // Start is called before the first frame update
    void Start()
    {
        cc = GameObject.FindGameObjectWithTag("CC").GetComponent<CheckpointController>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Player.transform.position = cc.lastCheckpointPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //could change back to using levelname if needed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
