using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepAudio : MonoBehaviour
{
    public GameObject player;
    public PlayerController _moveScript;
    public PlayAudio randClip;
    public float playInterval;
    private AudioSource source;
    public float resetTime;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        resetTime = playInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.D) |
            Input.GetKey(KeyCode.LeftArrow) | Input.GetKey(KeyCode.RightArrow)) & _moveScript.onGround)
        {
            resetTime -= Time.deltaTime;

            if (resetTime <= 0)
            {
                source.PlayOneShot(randClip.GetRandomClip());
                //source.Play();
                resetTime = playInterval;
            }
        }
            
    }
}