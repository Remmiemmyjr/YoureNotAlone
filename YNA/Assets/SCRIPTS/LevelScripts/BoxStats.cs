//*************************************************
// Project: We're Tethered Together
// File: BoxStats.cs
// Author/s: Emmy Berg
//           Mike Doeren
//
// Desc: Keeps track of box info
//
// Notes:
//  - good gamer internet???- 
//  - who wrote that
//
// Last Edit: 8/15/2023
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
    public Material shimmerMat;
    public Color canGrab;
    public Color isGrabbed;
    SpriteRenderer sr;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        GetComponent<HingeJoint2D>().enabled = false;
        sr = GetComponent<SpriteRenderer>();
        SetNormalMat();
    }


    ////////////////////////////////////////////////////////////////////////
    // SET NORMAL MAT ======================================================
    // Reset the box back to its normal material, and clear outline color
    public void SetNormalMat()
    {
        sr.material = boxMat;
        outlineMat.SetColor("_Color_Outline", Color.clear);
    }


    ////////////////////////////////////////////////////////////////////////
    // SET OUTLINE MAT =====================================================
    // Turn on the outline material for box. "colorSwap" dictates whether
    // the outline color should change (if partner is near box VS holding it)
    public void SetOutlineMat(bool colorSwap)
    {
        if (colorSwap)
        {
            sr.material = shimmerMat;
            //outlineMat.SetColor("_Color_Outline", isGrabbed);
        }
        else
            sr.material = shimmerMat;
    }
}
