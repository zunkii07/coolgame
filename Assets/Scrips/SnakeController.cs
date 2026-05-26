using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    private Vector2 nextDirection = Vector2.right;
    private List<Transform> bodySegments = new List<Transform>();
    public GameObject bodyPrefab;
    public float moveInterval = 0.2f;
    private float timer = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
            nextDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
            nextDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
            nextDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
            nextDirection = Vector2.right;

        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            timer = 0f;
            Move();
        }
    }

    void Move()
    {
        direction = nextDirection;
        float angle = 0f;
        if (direction == Vector2.up)
            angle = 180f;
        else if (direction == Vector2.down)
            angle = 0f;
        else if (direction == Vector2.left)
            angle = -90f;
        else if (direction == Vector2.right)
            angle = 90f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
        Vector2 prevPos = transform.position;
        transform.position = new Vector2(
            Mathf.Round(transform.position.x) + direction.x,
            Mathf.Round(transform.position.y) + direction.y
        );

        for (int i = 0; i < bodySegments.Count; i++)
        {
            Vector2 temp = bodySegments[i].position;
            bodySegments[i].position = prevPos;
            prevPos = temp;
        }
    }

    public void Grow()
    {
        GameObject segment = Instantiate(bodyPrefab);
        if (bodySegments.Count == 0)
            segment.transform.position = transform.position - (Vector3)direction;
        else
            segment.transform.position = bodySegments[bodySegments.Count - 1].position;

        bodySegments.Add(segment.transform);
    }

    public void Shrink()
    {
        if (bodySegments.Count > 0)
        {
            Destroy(bodySegments[bodySegments.Count - 1].gameObject);
            bodySegments.RemoveAt(bodySegments.Count - 1);
        }
        else
        {
            GameManager.Instance.GameOver();
        }
    }

    public void SetSpeed(float newInterval)
    {
        moveInterval = newInterval;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Body") || other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}