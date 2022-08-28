using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    private float camPos;
    public GameObject cam;
    public float parallaxEffect; 

    // Start is called before the first frame update
    void Start()
    {
        camPos = cam.transform.position.x;
        
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float deltaDist = (cam.transform.position.x - camPos) * parallaxEffect;
        

        startpos = transform.position.x;
        float camDist = (cam.transform.position.x - transform.position.x);


        if (camDist > length)
        {
            startpos += length;
        }

        else if (camDist < -length)
        {
            startpos -= length;
        }

        camPos = cam.transform.position.x;

        transform.position = new Vector3(startpos + deltaDist, transform.position.y, transform.position.z);
    }
}
