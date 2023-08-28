using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class god : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Partner")
        {
            // Steam Achievement
            Destroy(this.gameObject);
        }
    }
}
