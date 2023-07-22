using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DeathShader : MonoBehaviour
{
    GameObject manager;
    public Material stone;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager");
        stone.SetFloat("_ClippingVal", 1);
    }

    private void Start()
    {
        StartCoroutine(Lerp(0.95f));
    }

    private void Update()
    {
    }

    IEnumerator Lerp(float time)
    {
        float timeElapsed = 0;

        while (timeElapsed <= time)
        {
            float valueToLerp = Mathf.Lerp(1, 0.1f, timeElapsed / time);

            stone.SetFloat("_ClippingVal", valueToLerp);

            timeElapsed += Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
