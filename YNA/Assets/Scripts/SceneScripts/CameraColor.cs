//*************************************************
// Project: We're Tethered Together
// File: CameraColor.cs
// Author/s: I dunno who but thank you
//
// Desc: Change camera color to black
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Camera>().backgroundColor = Color.black;
    }
}
