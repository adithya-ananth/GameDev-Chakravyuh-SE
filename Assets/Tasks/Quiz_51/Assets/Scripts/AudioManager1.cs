using UnityEngine;

public class AudioManager1 : MonoBehaviour
{
    public AudioClip buttonClickSound;  // Assign your sound effect here
    private AudioSource audioSource;

    private void Awake()
    {
        // Ensure there's an AudioSource on this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        else
        {
            Debug.LogError("Button click sound is not assigned in the AudioManager.");
        }
    }
}
