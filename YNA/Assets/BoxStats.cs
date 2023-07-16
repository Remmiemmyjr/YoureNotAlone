using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<HingeJoint2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
