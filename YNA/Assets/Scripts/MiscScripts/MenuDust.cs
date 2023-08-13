using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuDust : MonoBehaviour
{
    private ParticleSystem particleSys;
    private bool isUsed = false;

    void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "MenuFloor" && !isUsed)
        {
            particleSys.Play();
            isUsed = true;
        }
    }
}
