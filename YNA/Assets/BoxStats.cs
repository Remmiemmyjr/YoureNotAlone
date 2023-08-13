//*************************************************
// Project: We're Tethered Together
// File: BoxStats.cs
// Author/s: Emmy Berg, Mike Doeren
//
// Desc: Keeps track of box info
//
// Notes:
// - 
//
// Last Edit: 8/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStats : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    public Material boxMat;
    public Material outlineMat;
    public Color canGrab;
    public Color isGrabbed;
    SpriteRenderer sr;
    // *********************************************************************

    void Awake()
    {
        GetComponent<HingeJoint2D>().enabled = false;
        sr = GetComponent<SpriteRenderer>();
        SetNormalMat();
    }

    public void SetNormalMat()
    {
        sr.material = boxMat;
    }

    public void SetOutlineMat(bool colorSwap)
    {
        sr.material = outlineMat;
        if (colorSwap)
            outlineMat.SetColor("_Color_Outline", isGrabbed);
        else
            outlineMat.SetColor("_Color_Outline", canGrab);
    }
}
