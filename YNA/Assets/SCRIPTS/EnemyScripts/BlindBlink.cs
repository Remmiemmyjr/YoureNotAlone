using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BlindBlink : MonoBehaviour
{
    public enum EyeType
    {
        ONE,
        TWO,
        THREE
    }
    public EyeType type;
    float animLength = 4;

    float min = 4;
    float max = 10;

    float waitTimer = 0;
    float currTime = 0;

    public Animator animator;


    private void Start()
    {
        waitTimer = Random.Range(1, 4);
        Invoke("CallBlink", waitTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if(currTime < waitTimer)
        {
            currTime += Time.deltaTime;
        }

        if(currTime >= waitTimer)
        {
            CallBlink();
        }
    }


    void CallBlink()
    {
        StartCoroutine(Blink());
    }

    public  IEnumerator Blink()
    {
        animator.Play("SlowBlink" + ((int)type + 1));
        Debug.Log("SlowBlink" +((int)type + 1));
        animLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength);
        currTime = 0;
        waitTimer = Random.Range(min, max);
    }
}
