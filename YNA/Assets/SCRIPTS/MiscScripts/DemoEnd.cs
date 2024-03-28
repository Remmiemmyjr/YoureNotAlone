using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoEnd : MonoBehaviour
{
    // Start is called before the first frame update

    public Image dark;

    bool inTransition = false;

    float timer = 10f;
    float currTime;

    private void Start()
    {
        PlayerPrefs.SetInt("currentLevelBuildIndex", 0);

        currTime = timer;
    }


    void Update()
    {
        if(currTime >= 0)
        {
            currTime -= Time.deltaTime;
        }

        if (currTime <= 0)
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

            dark.color = new Vector4(0, 0, 0, valueToLerp);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
}
