using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//README
//Create a sprite/gameobject and add this script, tag it with "Spawn"
//for every checkpoint after, repeat but tag with "Checkpoint"
//NOTE: the checkpoint tag doesnt actually do anything rn
public class Checkpoint : MonoBehaviour
{
    //private CheckpointController cc;
    public Sprite CheckOn;

    // Fancy little effects
    private ParticleSystem EmberPlayer;
    private UnityEngine.Rendering.Universal.Light2D CheckpointLight;

    [SerializeField]
    private float lightGrowRate = 1.0f;

    [SerializeField]
    private bool isSpawn = false;

    // Boolean to keep track of reached or not
    private bool checkReached = false;

    void Start()
    {
        //grab the controller
        //cc = GameObject.FindGameObjectWithTag("CC").GetComponent<CheckpointController>();

        EmberPlayer = GetComponent<ParticleSystem>();
        CheckpointLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        // Activate spawn lights on scene start
        if (isSpawn)
        {
            GetComponent<SpriteRenderer>().sprite = CheckOn;

            // Make sure extra items exist
            if (EmberPlayer && CheckpointLight && !checkReached)
            {
                // Play Particles
                EmberPlayer.Play();

                // Enable the light
                CheckpointLight.enabled = true;

                // Update value
                checkReached = true;
            }
        }
    }

    void Update()
    {
        // If the checkpoint has been activated and the light is less than intended, grow it
        if (checkReached && CheckpointLight.pointLightOuterRadius < 3)
        {
            CheckpointLight.pointLightOuterRadius += lightGrowRate * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //check for checkpoint reached by partner
        if(other.CompareTag("Partner"))
        {
            CheckpointController.lastCheckpointPos = transform.position;

            GetComponent<SpriteRenderer>().sprite = CheckOn;

            // Make sure extra items exist
            if (EmberPlayer && CheckpointLight && !checkReached)
            {
                // Play Particles
                EmberPlayer.Play();

                // Enable the light
                CheckpointLight.enabled = true;

                // Update value
                checkReached = true;
            }
        }
    }
}
