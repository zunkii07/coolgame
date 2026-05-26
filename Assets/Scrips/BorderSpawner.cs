using UnityEngine;

public class BorderSpawner : MonoBehaviour
{
    public GameObject wallPrefab;
    public int width = 19;
    public int height = 10;

    void Start()
    {
        CreateBorders();
    }

    void CreateBorders()
    {
        for (int x = -width / 2; x <= width / 2; x++)
        {
            CreateWall(new Vector2(x, height / 2));
            CreateWall(new Vector2(x, -height / 2));
        }

        for (int y = -height / 2 + 1; y < height / 2; y++)
        {
            CreateWall(new Vector2(-width / 2, y));
            CreateWall(new Vector2(width / 2, y));
        }
    }

    void CreateWall(Vector2 position)
    {
        Instantiate(wallPrefab, position, Quaternion.identity);
    }
}