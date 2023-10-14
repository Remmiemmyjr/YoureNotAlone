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
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public float speed;
    private float ogSpeed;

    public RectTransform destination;
    private RectTransform credits;
    public Image dark;

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
        
        if (Vector2.Distance(credits.position, destination.position) < 0.1f)
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
        StartCoroutine(Lerp(2));
        yield return new WaitForSeconds(2.5f);
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator Lerp(float time)
    {
        float timeElapsed = 0;

        while (timeElapsed <= time)
        {
            float valueToLerp = Mathf.Lerp(0, 1f, timeElapsed / time);

            dark.color = new Vector4(0,0,0, valueToLerp);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
}
