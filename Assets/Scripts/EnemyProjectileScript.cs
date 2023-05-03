using UnityEngine;
using static Direction;

public class EnemyProjectileScript : MonoBehaviour
{
    // Enemy's move speed
    public float speed = 4;
    public float minX = -20, maxX = 20;

    // Enemy move direction (-1 = left, 1 = right)
    public Direction direction = Right;

    private bool died = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(speed > 0 ? minX : maxX, transform.position.y);
        transform.Rotate(0, 0, direction == Right ? 180 : (direction == Up ? 270 : (direction == Down ? 90 : 0)));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check for direction change
        if (transform.position.x < minX || transform.position.x > maxX)
            TurnAround();

        // Move
        switch (direction)
        {
            case Right:
                transform.position = new Vector2(transform.position.x + speed * Time.fixedDeltaTime, transform.position.y);
                break;
            case Left:
                transform.position = new Vector2(transform.position.x - speed * Time.fixedDeltaTime, transform.position.y);
                break;
            case Up:
                transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.fixedDeltaTime);
                break;
            case Down:
                transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.fixedDeltaTime);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GolemController player;
        if (collision.gameObject.TryGetComponent<GolemController>(out player))
        {
            if (player.isAttacking)
            {
                // Player killed it!
                died = true;
                GetComponent<BoxCollider2D>().enabled = false;
                speed = 0;
            }
            else if (!died)
                // Player got kill'd by it!
                player.Die();
        }
    }

    // Do a flip
    void TurnAround()
    {
        switch (direction)
        {
            case Right:
                transform.position = new Vector2(transform.position.x + (minX - maxX), transform.position.y);
                break;
            case Left:
                transform.position = new Vector2(transform.position.x + (maxX - minX), transform.position.y);
                break;
            case Up:
                transform.position = new Vector2(transform.position.x, transform.position.y + (minX - maxX));
                break;
            case Down:
                transform.position = new Vector2(transform.position.x, transform.position.y + (maxX - minX));
                break;
        }
    }

    // Commit death
    void Die()
    {
        Destroy(gameObject);
    }
}
