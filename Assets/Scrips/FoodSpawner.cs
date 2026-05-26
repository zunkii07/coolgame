using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject normalFoodPrefab;
    public GameObject bonusFoodPrefab;
    public float spawnInterval = 3f;
    public int width = 22;
    public int height = 22;
    public LayerMask occupiedMask;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnFood();
        }
    }

    void SpawnFood()
    {
        Vector2 pos = GetFreePosition();
        float rand = Random.value;
        if (rand < 0.6f)
            Instantiate(normalFoodPrefab, pos, Quaternion.identity);
        else if (rand < 0.85f)
            Instantiate(bonusFoodPrefab, pos, Quaternion.identity);
    }

    Vector2 GetFreePosition()
    {
        int attempts = 0;
        while (attempts < 100)
        {
            int x = Random.Range(-width / 2 + 1, width / 2);
            int y = Random.Range(-height / 2 + 1, height / 2);
            Vector2 pos = new Vector2(x, y);
            Collider2D hit = Physics2D.OverlapCircle(pos, 0.4f, occupiedMask);
            if (hit == null)
            {
                return pos;
            }

            attempts++;
        }

        return Vector2.zero;
    }
}