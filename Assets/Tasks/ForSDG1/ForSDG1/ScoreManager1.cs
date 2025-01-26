using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager1 : MonoBehaviour
{
    public Text scoreText; // Reference to Score UI Text
    public Text missionPassedText; // Reference to Mission Passed Text
    public Text textToHide1; // First text to hide
    public Text textToHide2; // Second text to hide
    public AudioSource missionPassedAudio; // AudioSource for Mission Passed music
    private int score = 0;

    private void Start()
    {
        // Hide the Mission Passed text at the start
        missionPassedText.gameObject.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();

        if (score >= 65)
        {
            ShowMissionPassed();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "SCORE: " + score;
    }

    // private void ShowMissionPassed()
    // {
    //     // Display the Mission Passed text
    //     missionPassedText.gameObject.SetActive(true);

    //     // Hide the other two text objects
    //     textToHide1.gameObject.SetActive(false);
    //     textToHide2.gameObject.SetActive(false);

    //     // Play the Mission Passed music
    //     if (missionPassedAudio != null)
    //     {
    //         missionPassedAudio.Play();
    //     }
    // }

    private IEnumerator FadeOutText(Text textObject)
    {
        Color originalColor = textObject.color;
        while (textObject.color.a > 0)
        {
            textObject.color = new Color(originalColor.r, originalColor.g, originalColor.b, textObject.color.a - Time.deltaTime);
            yield return null;
        }
        textObject.gameObject.SetActive(false);
    }

    private void ShowMissionPassed()
    {
        PointsManager.IncrementPoints(25);
        StartCoroutine(PlayMissionMusicWithDelay());

        SceneManager.LoadScene("Map2");

        missionPassedText.gameObject.SetActive(true);
        StartCoroutine(FadeOutText(textToHide1));
        StartCoroutine(FadeOutText(textToHide2));

        if (missionPassedAudio != null)
        {
            missionPassedAudio.Play();
        }
    }

    private IEnumerator PlayMissionMusicWithDelay()
    {
        yield return new WaitForSeconds(1f); // Delay for 1 second
        if (missionPassedAudio != null)
        {
            missionPassedAudio.Play();
        }
    }
}