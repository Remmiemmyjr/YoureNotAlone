using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [HideInInspector]
    public bool isHidden;
    [HideInInspector]
    public bool isDead;



    private void Update()
    {
        //Debug.Log("Status: " + this.gameObject.name + "is hidden: " + isHidden);
    }
}
