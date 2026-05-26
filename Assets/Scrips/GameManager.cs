using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public float speedIncreaseInterval = 10f;
    public float speedIncreaseAmount = 0.01f;
    private int score = 0;
    private float speedTimer = 0f;
    private SnakeController snake;
    private bool gameStarted = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        snake = FindObjectOfType<SnakeController>();
        UpdateScoreUI();
        Time.timeScale = 0f;
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (!gameStarted)
            return;

        speedTimer += Time.deltaTime;

        if (speedTimer >= speedIncreaseInterval)
        {
            speedTimer = 0f;
            float newInterval = Mathf.Max(
                0.05f,
                snake.moveInterval - speedIncreaseAmount
            );

            snake.SetSpeed(newInterval);
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f;
        startPanel.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (score < 0)
            score = 0;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "—чЄт: " + score;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        if (finalScoreText != null)
        {
            finalScoreText.text = "‘инальный счЄт: " + score;
        }

        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}