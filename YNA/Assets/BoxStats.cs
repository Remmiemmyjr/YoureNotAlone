//*************************************************
// Project: We're Tethered Together
// File: BoxStats.cs
// Author/s: Emmy Berg
//
// Desc: Keeps track of box info
//
// Notes:
// - 
//
// Last Edit: 7/16/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStats : MonoBehaviour
{
    void Awake()
    {
        GetComponent<HingeJoint2D>().enabled = false;
    }
}
