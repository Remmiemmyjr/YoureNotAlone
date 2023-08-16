//*************************************************
// Project: We're Tethered Together
// File: PuzzleDoorProfile.cs
// Author/s: Emmy Berg
//
// Desc: Manage lighting and coloring of puzzle
//       door components
//
// Notes:
// -
//
// Last Edit: 8/11/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoorProfile : MonoBehaviour
{
    public GameObject gem;
    public GameObject gemLight;


    public void DisableGem(Color c)
    {
        gemLight.SetActive(false);
        gem.GetComponent<SpriteRenderer>().color = c;
    }    


    public void EnableGem(Color c)
    {
        gemLight.SetActive(true);
        gem.GetComponent<SpriteRenderer>().color = c;
        gemLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color = c;
    }
}
