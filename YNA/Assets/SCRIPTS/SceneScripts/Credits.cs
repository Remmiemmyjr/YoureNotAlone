//*************************************************
// Project: We're Tethered Together
// File: Credits.cs
// Author/s: Emmy Berg
//
// Desc: Credit scroll up to specific location
//
// Last Edit: 8/13/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public float speed;
    public RectTransform destination;
    private RectTransform credits;
    bool done = false;

    private void Start()
    {
        credits = GetComponent<RectTransform>();
    }


    void Update()
    {
        if(credits.position.y < destination.position.y && !done)
        {
            credits.position = Vector2.MoveTowards(credits.position, destination.position, speed * Time.deltaTime);
        }
        else
        {
            done = true;
        }
    }
}
