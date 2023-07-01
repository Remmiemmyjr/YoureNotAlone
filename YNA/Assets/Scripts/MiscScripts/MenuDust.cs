using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuDust : MonoBehaviour
{
    private ParticleSystem particleSys;

    void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "MenuFloor")
        {
            particleSys.Play();
        }
    }
}
