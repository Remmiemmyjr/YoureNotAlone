using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retether : MonoBehaviour
{
    public static bool relink;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Partner")
        {
            Info.grapple.startTetheredTogether = true;
            DontDestroyOnLoad(gameObject);
            relink = true;
        }
    }

}
