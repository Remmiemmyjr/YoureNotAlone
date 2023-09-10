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
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public float speed;
    private float ogSpeed;

    public RectTransform destination;
    private RectTransform credits;

    bool inTransition = false;

    private void Start()
    {
        credits = GetComponent<RectTransform>();

        ogSpeed = speed;
    }


    void Update()
    {
        if (credits.position.y < destination.position.y)
        {
            credits.position = Vector2.MoveTowards(credits.position, destination.position, speed * Time.deltaTime);
        }
        
        if (Vector2.Distance(credits.position, destination.position) < 1.0f)
        {
            if (!inTransition)
            {
                inTransition = true;
                StartCoroutine(ReturnToMenu());
            }
        }
    }

    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene("MainMenu");
    }
}
