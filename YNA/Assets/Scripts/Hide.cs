using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Partner")
        {
            collision.gameObject.GetComponent<Stats>().isHidden = true;
            collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Partner")
        {
            collision.gameObject.GetComponent<Stats>().isHidden = false;
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
