using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEyes : MonoBehaviour
{
    GameObject Player;
    GameObject Partner;

    bool timeToHide = false;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Partner = GameObject.FindGameObjectWithTag("Partner");
    }

    void Update()
    {

        if (timeToHide)
        {
            if(Player.GetComponent<Stats>().isHidden == false || Partner.GetComponent<Stats>().isHidden == false)
            {
                Debug.Log("YOURE DEAD");
                Player.GetComponent<Stats>().isDead = true;
                Partner.GetComponent<Stats>().isDead = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            timeToHide = !timeToHide;
            Debug.Log(timeToHide);
        }
    }
}
