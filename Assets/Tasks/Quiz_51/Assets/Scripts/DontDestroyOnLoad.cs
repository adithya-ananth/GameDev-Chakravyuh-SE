using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        // Ensure this GameObject persists across scenes
        DontDestroyOnLoad(gameObject);

        // Ensure only one instance of the background music exists
        if (FindObjectsByType<DontDestroyOnLoad>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
