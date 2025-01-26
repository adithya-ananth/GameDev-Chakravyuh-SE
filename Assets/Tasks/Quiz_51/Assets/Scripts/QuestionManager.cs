using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene management

[System.Serializable]
public class Question
{
    public string questionText;      // The question text
    public string[] options;         // Array of 3 options
    public int[] moneyEffects;       // Effects on Money for each option
    public int[] natureEffects;      // Effects on Nature Health for each option
    public string feedback;          // One feedback line for the entire question
}

public class QuestionManager : MonoBehaviour
{
    public List<Question> questions;          // List of all questions
    public TextMeshProUGUI questionText;      // Text field for the question
    public Button[] optionButtons;            // Buttons for the 3 options
    public TextMeshProUGUI moneyText;         // Text field to display Money
    public TextMeshProUGUI natureText;        // Text field to display Nature Health
    public TextMeshProUGUI feedbackText;      // Text field to display feedback

    private int money = 50;                   // Initial Money value
    private int natureHealth = 50;            // Initial Nature Health value
    private int currentQuestionIndex = -1;    // To track the current question number
    private bool isWaitingForNextQuestion = false; // Prevent multiple clicks during feedback

    private void Start()
    {
        InitializeUIReferences();
        UpdateUI();
        LoadNextQuestion();
    }

    private void InitializeUIReferences()
    {
        questionText = GameObject.Find("QuestionText")?.GetComponent<TextMeshProUGUI>();
        if (questionText == null)
            Debug.LogError("QuestionText GameObject not found or TextMeshProUGUI component missing.");

        moneyText = GameObject.Find("MoneyText")?.GetComponent<TextMeshProUGUI>();
        if (moneyText == null)
            Debug.LogError("MoneyText GameObject not found or TextMeshProUGUI component missing.");

        natureText = GameObject.Find("NatureText")?.GetComponent<TextMeshProUGUI>();
        if (natureText == null)
            Debug.LogError("NatureText GameObject not found or TextMeshProUGUI component missing.");

        feedbackText = GameObject.Find("FeedbackText")?.GetComponent<TextMeshProUGUI>();
        if (feedbackText == null)
            Debug.LogError("FeedbackText GameObject not found or TextMeshProUGUI component missing.");

        optionButtons = new Button[3];
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i] = GameObject.Find($"Option{i + 1}")?.GetComponent<Button>();
            if (optionButtons[i] == null)
                Debug.LogError($"Option{i + 1} GameObject not found or Button component missing.");
        }

        feedbackText.text = ""; // Clear feedback text initially
    }

    private void LoadNextQuestion()
    {
        if (isWaitingForNextQuestion) return; // Prevent overlapping calls

        // Check if game should end
        if (money <= 0 || natureHealth <= 0)
        {
            EndGame("Game Over!");
            return;
        }

        // Increment the question index
        currentQuestionIndex++;
        if (currentQuestionIndex >= questions.Count || (money >= 100 && natureHealth >= 100))
        {
            EndGame("Game Complete!");
            return;
        }

        // Get the current question
        Question q = questions[currentQuestionIndex];

        // Validate data
        if (q.options.Length != 3 || q.moneyEffects.Length != 3 || q.natureEffects.Length != 3)
        {
            Debug.LogError("Question data is not properly configured!");
            return;
        }

        // Set question text
        questionText.text = q.questionText;

        // Set up the buttons with options and listeners
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int localIndex = i; // Fix closure issue
            int moneyEffect = q.moneyEffects[i];
            int natureEffect = q.natureEffects[i];

            // Set button text and listener
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.options[i];
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(q, moneyEffect, natureEffect));
        }

        feedbackText.text = ""; // Clear feedback for the new question
    }

    private void OnOptionSelected(Question question, int moneyEffect, int natureEffect)
    {
        if (isWaitingForNextQuestion) return; // Prevent multiple selections

        isWaitingForNextQuestion = true; // Block further input during feedback

        // Apply effects
        ApplyEffects(moneyEffect, natureEffect);

        // Display feedback
        feedbackText.text = question.feedback;

        // Wait for 2 seconds, then load the next question
        StartCoroutine(WaitAndLoadNextQuestion());
    }

    private void ApplyEffects(int moneyEffect, int natureEffect)
    {
        Debug.Log($"Applying Effects: Money = {moneyEffect}, Nature Health = {natureEffect}");

        // Apply effects with constraints
        money = Mathf.Max(0, money + moneyEffect);                        // Ensure money is non-negative
        natureHealth = Mathf.Clamp(natureHealth + natureEffect, 0, 100);  // Clamp natureHealth between 0 and 100

        UpdateUI();
    }

    private IEnumerator WaitAndLoadNextQuestion()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        isWaitingForNextQuestion = false;   // Allow input again
        LoadNextQuestion();                 // Load the next question
    }

    private void UpdateUI()
    {
        moneyText.text = "Money: $" + money;
        natureText.text = "Nature Health: " + natureHealth + "%";
        Debug.Log($"Updated UI: Money = {money}, Nature Health = {natureHealth}");
    }

    private void EndGame(string message)
    {
        // Calculate the final score
        float score = (money / 20f) + (natureHealth / 20f);

        // Save the score using PlayerPrefs
        PlayerPrefs.SetFloat("FinalScore", score);
        PlayerPrefs.Save(); // Ensure the data is saved

        // Display the end message
        feedbackText.text = $"{message}\nScore: {score:F2}"; // Display score with 2 decimal places
        PointsManager.IncrementPoints((int)score);
        SceneManager.LoadScene("Map2");
    }

}
