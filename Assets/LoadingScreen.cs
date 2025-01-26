using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public Slider loadingBar; // Reference to the loading bar Slider
    public Text loadingText; // Reference to the loading text

    // Method to load a scene
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // Coroutine to handle asynchronous scene loading
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Optional: Wait for user input before activation

        while (!operation.isDone)
        {
            // Calculate the progress value (normalized between 0 and 1)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress; // Update the Slider value
            loadingText.text = $"Loading: {progress * 100:0}%"; // Update the text

            // When the scene is fully loaded but not yet activated
            if (operation.progress >= 0.9f)
            {
                loadingText.text = "Press Any Key to Continue";
                if (Input.anyKeyDown) // Wait for user input to activate the scene
                    operation.allowSceneActivation = true;
            }

            yield return null; // Wait for the next frame
        }
    }
}
