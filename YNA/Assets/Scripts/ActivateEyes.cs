using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEyes : MonoBehaviour
{
    GameObject Player;
    GameObject Partner;

    public GameObject[] Eyes;
    public Sprite closed;
    public Sprite halfway;
    public Sprite open;

    Animator eyeAnim;
    SpriteRenderer eyeRenderer;


    bool timeToHide = false;
    [HideInInspector]
    public bool canActivate = true;

    public float min = 5f;
    public float max = 10f;

    float maxTime;
    float currTime;

    void Start()
    {
        for(int i = 0; i < Eyes.Length; i++)
        {
            eyeRenderer = Eyes[i].GetComponent<SpriteRenderer>();
            eyeRenderer.sprite = closed;

            eyeAnim = Eyes[i].GetComponent<Animator>();
            eyeAnim.Play("EyeballClosed");
            eyeAnim.SetBool("isOpen", timeToHide);
        }

        Player = GameObject.FindGameObjectWithTag("Player");
        Partner = GameObject.FindGameObjectWithTag("Partner");

        SelectNewTime();

    }

    void Update()
    {
        if (canActivate)
        {
            if (currTime >= 0)
            {
                timeToHide = false;

                for (int i = 0; i < Eyes.Length; i++)
                {
                    eyeRenderer = Eyes[i].GetComponent<SpriteRenderer>();
                    eyeRenderer.sprite = closed;

                    eyeAnim = Eyes[i].GetComponent<Animator>();
                    eyeAnim.SetBool("isOpen", timeToHide);
                    eyeAnim.Play("EyeballClosed");
                }

                currTime -= Time.deltaTime;
            }

            if (currTime <= 3.5f && currTime > 0)
            {
                for (int i = 0; i < Eyes.Length; i++)
                {
                    eyeRenderer = Eyes[i].GetComponent<SpriteRenderer>();
                    eyeRenderer.sprite = halfway;

                    eyeAnim = Eyes[i].GetComponent<Animator>();
                    eyeAnim.Play("EyeballHalfway");
                }
            }

            if (currTime <= 0)
            {
                timeToHide = true;

                for (int i = 0; i < Eyes.Length; i++)
                {
                    eyeRenderer = Eyes[i].GetComponent<SpriteRenderer>();
                    eyeRenderer.sprite = open;

                    eyeAnim = Eyes[i].GetComponent<Animator>();
                    eyeAnim.SetBool("isOpen", timeToHide);
                    eyeAnim.Play("EyeballOpen");
                }

                StartCoroutine(LookAround());
            }

            if (timeToHide)
            {
                KillCheck();
            }

        }

        else 
        {
            currTime = maxTime;
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
        yield return new WaitForSeconds(3.5f);
        SelectNewTime();
    }
}
