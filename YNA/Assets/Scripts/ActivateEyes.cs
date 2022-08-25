using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEyes : MonoBehaviour
{
    GameObject Player;
    GameObject Partner;

    bool timeToHide = false;
    public bool canActivate = true;

    public float min = 5f;
    public float max = 10f;

    float maxTime;
    float currTime;

    void Start()
    {
        //get rid of l8r
        gameObject.GetComponent<Camera>().backgroundColor = Color.cyan;

        Player = GameObject.FindGameObjectWithTag("Player");
        Partner = GameObject.FindGameObjectWithTag("Partner");

        SelectNewTime();

    }

    void Update()
    {
        Debug.Log(currTime);

        if (canActivate)
        {
            if (currTime >= 0)
            {
                gameObject.GetComponent<Camera>().backgroundColor = Color.cyan;
                timeToHide = false;
                currTime -= Time.deltaTime;
            }
            if (currTime <= 3 && currTime > 0)
            {
                //get rid of l8r
                gameObject.GetComponent<Camera>().backgroundColor = Color.gray;
                Debug.Log("Get ready...");
            }
            if (currTime <= 0)
            {
                gameObject.GetComponent<Camera>().backgroundColor = Color.black;
                timeToHide = true;
                StartCoroutine(LookAround());
            }


            if (timeToHide)
            {
                KillCheck();
            }

        }
    }

    void KillCheck()
    {
        if (Player.GetComponent<Stats>().isHidden == false || Partner.GetComponent<Stats>().isHidden == false)
        {
            Debug.Log("YOURE DEAD");
            Player.GetComponent<Stats>().isDead = true;
            Partner.GetComponent<Stats>().isDead = true;
        }
    }

    void SelectNewTime()
    {
        maxTime = Random.Range(min, max);
        currTime = maxTime;
        Debug.Log("new max time: " + maxTime);
    }

    IEnumerator LookAround()
    {
        yield return new WaitForSeconds(5f);
        SelectNewTime();
    }
}
