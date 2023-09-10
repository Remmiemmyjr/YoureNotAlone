using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEyes : MonoBehaviour
{
    public ActivateEyes eyeManager;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            eyeManager.enabled = false;
        }
    }
}
