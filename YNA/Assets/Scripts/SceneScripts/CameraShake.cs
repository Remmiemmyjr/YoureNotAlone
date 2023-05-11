using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake manager;

    // Start is called before the first frame update
    void Start()
    {
        if(manager == null)
        {
            manager = this;
        }   
    }

    // Update is called once per frame
    public void Shake(CinemachineImpulseSource source)
    {
        source.GenerateImpulseWithForce(0.1f);
    }
}
