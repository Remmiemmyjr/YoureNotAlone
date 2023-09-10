using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Untether : MonoBehaviour
{
    public AudioSource iamhere;

    [SerializeField]
    private AudioClip eyembiance;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Partner")
        {
            Info.grapple.Tethered(false);
            iamhere.PlayOneShot(eyembiance);
        }
    }
}
