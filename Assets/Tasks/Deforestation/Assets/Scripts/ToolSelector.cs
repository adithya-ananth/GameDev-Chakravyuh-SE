using UnityEngine;

public class ToolSelector : MonoBehaviour
{
    public string toolName; // Tool name: "Axe" or "Fire"
    public AudioClip selectSound; // The audio clip to play when the tool is selected
    private AudioSource audioSource; // Reference to the AudioSource component

    public TreeInteractionManager treeManager; // Reference to TreeInteractionManager

    void Start()
    {
        // Ensure the GameObject has an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the AudioSource to play the assigned clip (optional)
        audioSource.playOnAwake = false; // Prevent the sound from playing automatically
    }

    public void PlayButtonSound()
    {
        // Play the assigned sound
        if (selectSound != null)
        {
            audioSource.PlayOneShot(selectSound);
        }
        else
        {
            Debug.LogWarning("No audio clip assigned for this tool!");
        }
    }

    void OnMouseDown()
    {
        // Play sound when the tool is selected
        PlayButtonSound();

        // Notify the TreeInteractionManager about the selected tool
        if (treeManager != null)
        {
            treeManager.SelectTool(toolName);
        }
        else
        {
            Debug.LogError("TreeManager is not assigned in ToolSelector.");
        }
    }
}
