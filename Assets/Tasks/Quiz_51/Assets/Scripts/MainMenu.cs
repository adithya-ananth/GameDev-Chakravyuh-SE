using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Assign this in the Inspector

    //private void Start()
    //{
    //    // Retrieve the score from PlayerPrefs
    //    if (PlayerPrefs.HasKey("FinalScore"))
    //    {
    //        float score = PlayerPrefs.GetFloat("FinalScore");
    //        scoreText.text = $"Last Score: {score:F2}"; // Display score with 2 decimal places
    //    }
    //    else
    //    {
    //        scoreText.text = "Last Score: N/A";
    //    }
    //}

    public void StartGame()
    {
        SceneManager.LoadScene("QuizMain"); // Replace "GameScene" with the name of your game scene
    }

    public void QuitGame()
    {
        Application.Quit(); // Quits the application
        Debug.Log("Game Quit");
    }
}
