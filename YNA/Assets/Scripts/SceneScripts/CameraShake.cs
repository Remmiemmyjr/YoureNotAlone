//*************************************************
// Project: We're Tethered Together
// File: CameraShake.cs
// Author/s: Emmy Berg
//
// Desc: Manager for camera shake impulse
//
// Last Edit: 5/11/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public static CameraShake manager;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        if(manager == null)
        {
            manager = this;
        }   
    }


    ////////////////////////////////////////////////////////////////////////
    // SHAKE ===============================================================
    // Perform the *shake*
    public void Shake(CinemachineImpulseSource source, float force)
    {
        source.GenerateImpulseWithForce(force);
    }
}
