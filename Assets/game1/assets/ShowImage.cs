using System.Collections; // For IEnumerator
using UnityEngine;        // For Unity-specific types and functions

public class ShowImage : MonoBehaviour
{
    public GameObject imageObject; // Drag your image GameObject here
    public float displayTime = 10f; // Duration to show the image

    void Start()
    {
        // Pause the game
        Time.timeScale = 0;

        // Start the coroutine to display the image
        StartCoroutine(ShowAndHideImage());
    }

    private IEnumerator ShowAndHideImage()
    {
        // Show the image
        imageObject.SetActive(true);

        // Wait for the specified time
        yield return new WaitForSecondsRealtime(displayTime); // Use WaitForSecondsRealtime to ignore time scale

        // Hide the image
        imageObject.SetActive(false);

        // Resume the game
        Time.timeScale = 1;
    }
}
