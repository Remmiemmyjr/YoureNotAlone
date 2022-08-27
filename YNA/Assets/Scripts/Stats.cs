using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
    [HideInInspector]
    public bool isHidden;
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public static string currLevel;

    Vector3 newZPos;

    void Start()
    {
        newZPos = new Vector3(transform.position.x, transform.position.y, -1);
        transform.position = newZPos;

        //this.GetComponent<Rigidbody2D>().sl = 0.0f;
    }

    void Update()
    {
        //Debug.Log("Status: " + this.gameObject.name + "is hidden: " + isHidden);
        if (this.GetComponent<Rigidbody2D>().IsSleeping())
        {
            this.GetComponent<Rigidbody2D>().WakeUp();
        }

        if(isDead)
        {
            //currLevel = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //SceneManager.LoadScene("GameOver");
        }
    }
}
