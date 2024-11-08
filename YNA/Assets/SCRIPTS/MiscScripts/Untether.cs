// Project: We're Tethered Together
// File: Untether.cs
// Author/s: Emmy Berg
//
// Desc: Forcefully untether the partner
//
// Notes:
//  - traumatize everyone
//
// Last Edit: 8/19/2024
//
//*************************************************

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Untether : MonoBehaviour
{
    public AudioSource iamhere;
    CinemachineImpulseSource impulse;
    public GameObject annoyingPlatformToDisable;

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
            CameraShake.manager.Shake(impulse, 1f);
            annoyingPlatformToDisable.SetActive(false);
            StartCoroutine(ControllerRumble.ControllerRumbleFX(1.5f, 2.5f, 3f));
            GetComponent<SteamForceAwardAchievement>().AwardAchievement();
        }
    }
}
