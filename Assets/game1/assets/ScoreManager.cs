using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Score Settings")]
    public TextMeshProUGUI scoreText; // To display the score
    public int maxScore = 100; // Target score to end the game

    [Header("Time Settings")]
    public TextMeshProUGUI finalScoreText; // To display the final score
    private float startTime; // Time when the game starts

    private int score = 0; // Current score

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Record the time when the game starts
        startTime = Time.time;
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();

        if (score >= maxScore)
        {
            CalculateFinalScore();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void CalculateFinalScore()
    {
        // Calculate total time taken
        float totalTime = Time.time - startTime;

        int finalScore;

        // Apply if-else logic to calculate the score percentage
        if (totalTime < 15f)
        {
            // If time is less than 15 seconds, score is 20% of maxScore
            finalScore = Mathf.RoundToInt(maxScore * 0.2f);
        }
        else if (totalTime >= 15f && totalTime <= 30f)
        {
            // If time is between 15 and 30 seconds, score is 15% of maxScore
            finalScore = Mathf.RoundToInt(maxScore * 0.15f);
        }
        else
        {
            // If time is greater than 30 seconds, score is 10% of maxScore
            finalScore = Mathf.RoundToInt(maxScore * 0.1f);
        }

        PointsManager.IncrementPoints(finalScore);
        SceneManager.LoadScene("Map3");

        // Display final score
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {finalScore}";
        }

        Debug.Log($"Game Over! Time Taken: {totalTime:F2}s, Final Score: {finalScore}");
    }
}
