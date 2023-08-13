using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public float speed;
    public RectTransform destination;
    private RectTransform credits;
    bool done = false;

    private void Start()
    {
        credits = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(credits.position.y < destination.position.y && !done)
        {
            credits.position = Vector2.MoveTowards(credits.position, destination.position, speed * Time.deltaTime);
        }
        else
        {
            done = true;
        }
    }
}
