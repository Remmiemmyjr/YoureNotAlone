//*************************************************
// Project: We're Tethered Together
// File: DeathShader.cs
// Author/s: Emmy Berg
//
// Desc: Gradually make stone overlay visible by
//       modifying the clipping value of the
//       "dissolve" (stone) shader
//
// Notes:
// -
//
// Last Edit: 8/5/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DeathShader : MonoBehaviour
{
    public Material stone;

    private void Awake()
    {
        stone.SetFloat("_ClippingVal", 1);
    }

    public IEnumerator Lerp(float time)
    {
        float timeElapsed = 0;

        while (timeElapsed <= time)
        {
            float valueToLerp = Mathf.Lerp(1, 0.001f, timeElapsed / time);

            stone.SetFloat("_ClippingVal", valueToLerp);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
}
