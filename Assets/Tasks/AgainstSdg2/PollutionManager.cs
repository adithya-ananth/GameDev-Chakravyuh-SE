using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PollutionManager : MonoBehaviour
{
    public Slider pollutionSlider; // Assign the UI Slider for pollution
    public TextMeshProUGUI gameOverText; // Assign the TextMeshPro object
    public float maxPollution = 70f; // Max pollution level
    public float gameDuration = 40f; // Total time for the game

    private float currentPollution = 0f; // Current pollution level
    private float elapsedTime = 0f; // Time elapsed in the game
    private bool gameEnded = false; // Track if the game has ended

    void Start()
    {
        if (pollutionSlider == null)
        {
            Debug.LogError("PollutionSlider is not assigned! Ensure a UI Slider is assigned in the Inspector.");
        }
        else
        {
            pollutionSlider.value = 0; // Initialize the slider
        }

        if (gameOverText == null)
        {
            Debug.LogError("GameOverText is not assigned! Assign a TextMeshProUGUI object in the Inspector.");
        }
        else
        {
            gameOverText.text = ""; // Clear the text initially
            gameOverText.gameObject.SetActive(false); // Disable it initially
        }
    }

    void Update()
    {
        if (gameEnded) return;

        // Update the elapsed time
        elapsedTime += Time.deltaTime;

        // Check if the time limit has been reached
        if (elapsedTime >= gameDuration)
        {
            CheckWinCondition();
        }
    }

    public void IncreasePollution(float amount)
    {
        if (gameEnded) return;

        currentPollution = Mathf.Clamp(currentPollution + amount, 0, maxPollution);

        if (pollutionSlider != null)
        {
            pollutionSlider.value = currentPollution / maxPollution;
        }

        Debug.Log($"Current Pollution: {currentPollution}/{maxPollution}");

        if (currentPollution >= maxPollution)
        {
            gameEnded = true;
            EndGame(true);
        }
    }

    void CheckWinCondition()
    {
        gameEnded = true;

        if (currentPollution >= maxPollution)
        {
            EndGame(true);
        }
        else
        {
            EndGame(false);
        }
    }

    void EndGame(bool didWin)
    {
        // Display the game over text
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true); // Activate the text

            if (didWin)
            {
                gameOverText.text = "You WON!";
                gameOverText.color = Color.green;
            }
            else
            {
                gameOverText.text = "Game Over! You LOST!";
                gameOverText.color = Color.red;
            }
        }

        // Calculate the score
        int scorePercentage = CalculateScore(didWin);
        Debug.Log(didWin ? $"Game Over: You WON! Score: {scorePercentage}%" : $"Game Over: You LOST! Score: {scorePercentage}%");

        PointsManager.IncrementPoints(-scorePercentage);
        // Stop the game, but keep UI updates active
        StartCoroutine(PauseGameWithUI());

        SceneManager.LoadScene("Map2");
    }

    int CalculateScore(bool didWin)
    {
        if (!didWin) return 0; // Return 0% if the player lost

        // Calculate the winning score
        float timeFactor = (gameDuration - elapsedTime) / gameDuration * 100f;
        float scorePercentage = 50f + timeFactor;
        int value = (int)Mathf.Clamp(scorePercentage, 0f, 100f); // Ensure the score is within the 0-100% range

        return value/5;
    }

    private System.Collections.IEnumerator PauseGameWithUI()
    {
        // Wait for a frame to ensure UI updates
        yield return null;

        // Freeze the game without freezing UI animations
        Time.timeScale = 0;
    }
}