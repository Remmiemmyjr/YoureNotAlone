// Project: We're Tethered Together
// File: BoxParentingManager.cs
// Author/s: Corbyn LaMar
//
// Desc: Child the box to the moving platform
//
// Notes:
//  - 
//
// Last Edit: 5/4/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxParentingManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Box is moving with moving platform and not being held
        if (transform.parent)
        {
            // Player is within the area
            if (collision.tag == "Player")
            {
                collision.transform.SetParent(transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Player is within the area
        if (collision.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }
}
