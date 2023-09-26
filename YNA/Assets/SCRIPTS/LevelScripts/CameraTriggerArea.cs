using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerArea : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;

    private float startSize;

    [SerializeField]
    private float endSize;

    [SerializeField]
    private float zoomTime;

    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    private void Start()
    {
        //vcam = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;

        startSize = vcam.m_Lens.OrthographicSize;
    }

    ////////////////////////////////////////////////////////////////////////
    // TRIGGER ENTER =======================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(Zoom(zoomTime, vcam.m_Lens.OrthographicSize, endSize));
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // TRIGGER EXIT ========================================================
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(Zoom(zoomTime, vcam.m_Lens.OrthographicSize, startSize));
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // ZOOM IN =============================================================
    public IEnumerator Zoom(float time, float current, float target)
    {
        float timeElapsed = 0;

        while (timeElapsed <= time)
        {
            float valueToLerp = Mathf.Lerp(current, target, timeElapsed / time);

            vcam.m_Lens.OrthographicSize = valueToLerp;

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
}
