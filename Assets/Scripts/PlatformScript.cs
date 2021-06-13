using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Direction;

public class PlatformScript : MonoBehaviour
{
    public float speed, distance;
    public Direction direction;

    private Vector2 defaultPosition;
    private bool activated, settled;

    private Rigidbody2D rb;

    void Start()
    {
        activated = false;
        rb = GetComponent<Rigidbody2D>();
        defaultPosition = rb.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activated && !settled)
        {
            switch (direction)
            {
                case Right:
                    rb.velocity = new Vector2(speed, 0);
                    settled = rb.position.x >= defaultPosition.x + distance;
                    break;
                case Up:
                    rb.velocity = new Vector2(0, speed);
                    settled = rb.position.y >= defaultPosition.y + distance;
                    break;
                case Left:
                    rb.velocity = new Vector2(-speed, 0);
                    settled = rb.position.x <= defaultPosition.x - distance;
                    break;
                case Down:
                    rb.velocity = new Vector2(0, -speed);
                    settled = rb.position.y <= defaultPosition.y - distance;
                    break;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    public void activate()
    {
        activated = true;
    }
}
