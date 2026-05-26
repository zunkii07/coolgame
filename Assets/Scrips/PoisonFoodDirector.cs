using System.Collections.Generic;
using UnityEngine;

public class PoisonFoodDirector : MonoBehaviour
{
    public GameObject poisonPrefab;
    public Transform snakeHead;
    public int radius = 5;
    public float spawnInterval = 10f;
    public float lifeTime = 4f;
    public LayerMask occupiedMask;
    private float timer = 0f;
    public int width = 19;
    public int height = 10;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawnPoison();
        }
    }

    bool InBounds(Vector2 pos)
    {
        return pos.x > -width / 2 + 1 &&
               pos.x < width / 2 &&
               pos.y > -height / 2 + 1 &&
               pos.y < height / 2;
    }

    void TrySpawnPoison()
    {
        Vector2? pos = FindValidPosition();
        if (pos != null)
        {
            GameObject poison =
                Instantiate(poisonPrefab, pos.Value, Quaternion.identity);

            Destroy(poison, lifeTime);
        }
    }

    Vector2? FindValidPosition()
    {
        List<Vector2> candidates = new List<Vector2>();
        Vector2 headPos = snakeHead.position;
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Vector2 pos = new Vector2(
                    Mathf.Round(headPos.x + x),
                    Mathf.Round(headPos.y + y)
                );

                if (!InBounds(pos))
                    continue;
                if (Physics2D.OverlapPoint(pos, occupiedMask) == null)
                {
                    candidates.Add(pos);
                }
            }
        }

        if (candidates.Count == 0)
            return null;
        Shuffle(candidates);
        foreach (Vector2 candidate in candidates)
        {
            if (IsSafe(candidate))
                return candidate;
        }

        return null;
    }

    bool IsSafe(Vector2 poisonPos)
    {
        Queue<Vector2> queue = new Queue<Vector2>();
        HashSet<Vector2> visited =
            new HashSet<Vector2>();
        Vector2 start =
            new Vector2(
                Mathf.Round(snakeHead.position.x),
                Mathf.Round(snakeHead.position.y)
            );

        queue.Enqueue(start);
        visited.Add(start);
        int maxSteps = 100;
        while (queue.Count > 0 && maxSteps-- > 0)
        {
            Vector2 current = queue.Dequeue();
            if (Vector2.Distance(current, poisonPos) > radius * 2)
                return true;

            Vector2[] dirs =
            {
                Vector2.up,
                Vector2.down,
                Vector2.left,
                Vector2.right
            };

            foreach (Vector2 dir in dirs)
            {
                Vector2 next = current + dir;
                if (visited.Contains(next))
                    continue;
                if (Physics2D.OverlapPoint(next, occupiedMask))
                    continue;
                visited.Add(next);
                queue.Enqueue(next);
            }
        }

        return false;
    }

    void Shuffle(List<Vector2> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector2 temp = list[i];
            int randomIndex =
                Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}