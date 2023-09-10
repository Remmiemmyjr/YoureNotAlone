using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEyes : MonoBehaviour
{
    public ActivateEyes eyeManager;

    [SerializeField]
    private bool enableEyes = false;

    [SerializeField]
    private string targetTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == targetTag)
        {
            eyeManager.enabled = enableEyes;
        }
    }
}
