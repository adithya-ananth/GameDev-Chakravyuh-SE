using UnityEngine;
using UnityEngine.UI;
using TMPro; // If you're using TextMeshPro

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider pollutionSlider; // Pollution slider to track pollution levels
    public TextMeshProUGUI winText; // Text to display on winning
    public TextMeshProUGUI loseText; // Text to display on losing

    [Header("Game Settings")]
    public float gameTimeLimit = 30f; // Game time limit in seconds

    private float elapsedTime = 0f; // Tracks elapsed time
    private bool gameEnded = false;

    void Start()
    {
        // Initialize the slider and UI
        pollutionSlider.value = 0f;
        pollutionSlider.maxValue = 1f; // Ensure the max value matches your requirement
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        // Track elapsed time
        elapsedTime += Time.deltaTime;

        // Check game end conditions
        CheckGameEnd();
    }

    public void IncreasePollution(float amount)
    {
        if (gameEnded) return;

        pollutionSlider.value += amount;

        // Check if pollution has reached the maximum
        if (pollutionSlider.value >= pollutionSlider.maxValue)
        {
            WinGame();
        }
    }

    private void CheckGameEnd()
    {
        // Check if the game timer has run out
        if (elapsedTime >= gameTimeLimit)
        {
            LoseGame();
        }
    }

    private void WinGame()
    {
        gameEnded = true;
        winText.gameObject.SetActive(true);
        Debug.Log("You Win!");
    }

    private void LoseGame()
    {
        gameEnded = true;
        loseText.gameObject.SetActive(true);
        Debug.Log("You Lose!");
    }
}
