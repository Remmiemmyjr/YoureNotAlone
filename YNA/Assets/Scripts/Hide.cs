using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Stats>().isHidden = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Stats>().isHidden = false;
    }
}
