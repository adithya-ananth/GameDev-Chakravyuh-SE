using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public GameObject objectPrefab;     // Prefab to spawn (assign in Inspector)
    public Transform[] spawnPoints;    // Array of spawn points (assign in Inspector)
    public int maxSpawns = 2;          // Maximum number of objects to spawn
    public Color disabledColor = Color.gray; // Color for disabled button

    private int spawnCount = 0;        // Tracks how many objects have been spawned
    private SpriteRenderer buttonSpriteRenderer; // Reference to the button's sprite renderer

    public ClickableWireSpawner wireSpawner;

    public int points = 0; // Points for the subgame

    private void Start()
    {
        // Get the SpriteRenderer of the clickable button
        buttonSpriteRenderer = GetComponent<SpriteRenderer>();
        if (buttonSpriteRenderer == null)
        {
            Debug.LogWarning("No SpriteRenderer found on this object!");
        }
    }

    private void Update()
    {
        points = wireSpawner.wirePoints;

        Debug.Log(points);

        if(points == 20)
        {
            PointsManager.IncrementPoints(10);
            SceneManager.LoadScene("Map3");
        }
    }

    private void OnMouseDown()
    {
        // Check if spawn limit is reached
        if (spawnCount >= maxSpawns)
        {
            Debug.Log("Max spawn limit reached for " + gameObject.name);
            return;
        }

        // Spawn the object at the next spawn point
        if (spawnPoints.Length > spawnCount)
        {
            Instantiate(objectPrefab, spawnPoints[spawnCount].position, Quaternion.identity);
            spawnCount++;

            // Check if spawn limit is reached after spawning
            if (spawnCount >= maxSpawns)
            {
                points += 10;
                DisableButton();
            }

        }
        else
        {
            Debug.LogWarning("Not enough spawn points assigned!");
        }
    }

    private void DisableButton()
    {
        // Change the button color to indicate it is disabled
        if (buttonSpriteRenderer != null)
        {
            buttonSpriteRenderer.color = disabledColor;
        }

        Debug.Log("Button is now disabled for " + gameObject.name);
    }
}
