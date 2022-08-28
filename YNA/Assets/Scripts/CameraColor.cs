using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Camera>().backgroundColor = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
