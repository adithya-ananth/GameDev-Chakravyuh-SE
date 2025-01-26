using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SubmarineCollision : MonoBehaviour
{
    public Text scoreText;
    public GameObject gameWinPanel;
    public Text gameWinScoreText; // Add a new Text UI element for displaying score on the win panel
    private int score = 0;

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("Score Text is not assigned in the Inspector!");
        }

        if (gameWinPanel != null)
        {
            gameWinPanel.SetActive(false);
        }

        if (gameWinScoreText == null)
        {
            Debug.LogError("Game Win Score Text is not assigned in the Inspector!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Trash"))
        {
            score += 10;
            Destroy(other.gameObject);
            UpdateScore();
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            RestartGame();
        }
        else if (other.gameObject.CompareTag("Map"))
        {
            GameWin();
        }
    }

    void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    void GameWin()
    {
        if (gameWinPanel != null)
        {
            gameWinPanel.SetActive(true);
        }

        if (gameWinScoreText != null)
        {
            gameWinScoreText.text = "Your Score is " + score.ToString(); // Display the score on the win panel
            PointsManager.IncrementPoints(score);
            SceneManager.LoadScene("Map3");
        }

        Time.timeScale = 0f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
