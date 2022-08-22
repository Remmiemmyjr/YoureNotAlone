using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [HideInInspector]
    public bool isHidden;
    [HideInInspector]
    public bool isDead;

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
    }
}
