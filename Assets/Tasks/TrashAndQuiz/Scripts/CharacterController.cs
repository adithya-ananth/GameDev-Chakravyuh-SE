using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    public int score = 0;
    public UnityEngine.UI.Text scoreText;
    public GameObject questionPanel;
    public UnityEngine.UI.Text questionText;
    public InputField answerInput;
    public Button submitButton;
    public GameObject gameOverPanel;
    public UnityEngine.UI.Text gameOverText;
    public Button restartButton;

    private Trash1 currentTrash;
    private bool isQuestionActive = false;

    void Start()
    {
        questionPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        submitButton.onClick.AddListener(OnSubmit);
        restartButton.onClick.AddListener(RestartGame);
        UpdateScore();
    }

    void Update()
    {
        if (AllTrashCollected())
        {
            if (score >= 50)
            {
                EndGame("You Win!");
            }
            else
            {
                EndGame("You Lose!");
            }
        }

        if (!isQuestionActive)
        {
            HandleMovement();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Trash1 trash = other.GetComponent<Trash1>();
        if (trash != null && !trash.isCollected)
        {
            AskQuestion(trash);
        }
    }

    private void AskQuestion(Trash1 trash)
    {
        currentTrash = trash;
        questionPanel.SetActive(true);
        questionText.text = trash.question;
        isQuestionActive = true;  // Disable movement when question is active
    }

    private void OnSubmit()
    {
        string playerAnswer = answerInput.text;

        if (playerAnswer == currentTrash.answer)
        {
            score += 10;
            UpdateScore();
        }
        currentTrash.isCollected = true;
        Destroy(currentTrash.gameObject);
        questionPanel.SetActive(false);
        answerInput.text = "";
        isQuestionActive = false;  // Enable movement after question is answered
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    private void EndGame(string result)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = result;
        isQuestionActive = true;  // Prevent further movement when the game ends
        PointsManager.IncrementPoints(score);
        SceneManager.LoadScene("Map3");
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private bool AllTrashCollected()
    {
        Trash1[] allTrash = UnityEngine.Object.FindObjectsByType<Trash1>(FindObjectsSortMode.None);
        foreach (Trash1 trash in allTrash)
        {
            if (!trash.isCollected)
            {
                return false;
            }
        }
        return true;
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0) * Time.deltaTime;
        transform.Translate(movement);
    }
}
