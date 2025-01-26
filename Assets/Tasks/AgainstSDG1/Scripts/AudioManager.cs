using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;


    [Header("Audio Clips")]
    public AudioClip bg;
    public AudioClip gameOver;
    public AudioClip gameStart;
    public AudioClip jump;

    private void Start()
    {
        musicSource.clip = bg;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            Debug.Log($"Playing SFX: {clip.name}");
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("AudioClip is null!");
        }
    }


}
