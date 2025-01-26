using System.Collections;
using UnityEngine;

public class DirtInteraction1 : MonoBehaviour
{
    public int clicksToRemove = 3; // Number of clicks required to remove the dirt
    private int currentClicks = 0; // Current click count
    private ScoreManager1 scoreManager; // Reference to a UI element
    private SpriteRenderer spriteRenderer; // Reference to a UI element
    private Vector3 initialScale; // Store the initial scale of the dirt

    public AudioClip clickSound;         // Assign in Inspector
    public AudioClip dirtRemovedSound;  // Assign in Inspector
    private AudioSource audioSource;

    private void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager1>();
        // debug call for ScoreManager1
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager1 not found!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        initialScale = transform.localScale; // Save the original size of the dirt

        // debug call for spriteRenderer
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on DirtInteraction1!");
        }

        // debug call for audioSource
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on DirtInteraction1!");
        }
    }

    private void OnMouseDown()
    {
        // Play click sound
        audioSource.PlayOneShot(clickSound);

        currentClicks++;
        Debug.Log("Dirt clicked! Remaining clicks: " + (clicksToRemove - currentClicks));

        // Scale the dirt down according to the number of clicks
        float scaleFactor = Mathf.Clamp01(1f - (float)currentClicks / clicksToRemove);
        transform.localScale = initialScale * scaleFactor;

        // Check if the dirt should be removed
        if (currentClicks >= clicksToRemove)
        {
            // Play dirt removed sound
            audioSource.PlayOneShot(dirtRemovedSound);
            StartCoroutine(FadeOutAndRemove());
        }
    }

    private IEnumerator FadeOutAndRemove()
    {
        float fadeDuration = 0.5f; // Duration of the fade-out effect in seconds
        float fadeSpeed = 1f / fadeDuration;
        Color originalColor = spriteRenderer.color;
        float alpha = originalColor.a;

        // Reduce alpha after each click
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Set alpha to 0
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        // Add score and destroy the dirt object
        scoreManager.AddScore(5);
        Destroy(gameObject);
    }
}