using UnityEngine;

public enum FoodType { Normal, Bonus, Poison }

public class Food : MonoBehaviour
{
    public FoodType foodType = FoodType.Normal;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SnakeController snake = other.GetComponent<SnakeController>();
            if (snake == null) return;

            if (foodType == FoodType.Normal)
            {
                snake.Grow();
                GameManager.Instance.AddScore(1);
            }
            else if (foodType == FoodType.Bonus)
            {
                snake.Grow();
                snake.Grow();
                GameManager.Instance.AddScore(3);
            }
            else if (foodType == FoodType.Poison)
            {
                snake.Shrink();
                GameManager.Instance.AddScore(-1);
            }

            Destroy(gameObject);
        }
    }
}
