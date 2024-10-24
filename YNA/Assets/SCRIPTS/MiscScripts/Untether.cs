using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Untether : MonoBehaviour
{
    public AudioSource iamhere;
    CinemachineImpulseSource impulse;

    [SerializeField]
    private AudioClip eyembiance;

    private void Awake()
    {
        impulse = GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Partner")
        {
            Info.grapple.Tethered(false);
            iamhere.PlayOneShot(eyembiance);
            CameraShake.manager.Shake(impulse, 0.25f);
        }
    }
}
