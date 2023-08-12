using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Vector2 movement;
    public float speed = 3f;

    public Transform[] wayPoints;
    public int startingPoint;

    int i;

    void Start()
    {
        transform.position = wayPoints[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, wayPoints[i].position) < 0.02f)
        {
            i++;

            if(i == wayPoints.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, wayPoints[i].position, speed * Time.deltaTime);

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Partner")
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Partner")
        {
            collision.transform.SetParent(null);
        }
    }
}
