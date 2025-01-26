using UnityEngine;

public class ClickableWireSpawner : MonoBehaviour
{
    public GameObject[] wirePrefabs;   // Array of wire prefabs to spawn (assign in Inspector)
    public Transform[] spawnPoints;   // Array of spawn points (assign in Inspector)
    public Color disabledColor = Color.gray; // Color for the button when disabled
    private int spawnCount = 0;       // Tracks how many wires have been spawned
    private SpriteRenderer buttonSpriteRenderer; // Reference to button sprite renderer
    public bool isAssembled = false; // Checks if all components have been used
    public int wirePoints = 0; // Points for the subgame

    private void Start()
    {
        // Get the SpriteRenderer of the clickable button
        buttonSpriteRenderer = GetComponent<SpriteRenderer>();
        if (buttonSpriteRenderer == null)
        {
            Debug.LogWarning("No SpriteRenderer found on this object!");
        }

        // Ensure wire prefabs are hidden initially
        foreach (GameObject wirePrefab in wirePrefabs)
        {
            SpriteRenderer wireSpriteRenderer = wirePrefab.GetComponent<SpriteRenderer>();
            if (wireSpriteRenderer != null)
            {
                wireSpriteRenderer.enabled = false; // Hide the wire initially
            }
            else
            {
                Debug.LogWarning("No SpriteRenderer found on wire prefab: " + wirePrefab.name);
            }
        }
    }

    private void OnMouseDown()
    {
        // Check if all wires have been spawned
        if (spawnCount >= wirePrefabs.Length || spawnCount >= spawnPoints.Length)
        {
            Debug.Log("All wires have been spawned for " + gameObject.name);
            DisableButton();
            return;
        }

        // Spawn the next wire prefab at the corresponding spawn point
        GameObject wireToSpawn = wirePrefabs[spawnCount];
        Transform spawnPoint = spawnPoints[spawnCount];

        // Instantiate and enable the wire's SpriteRenderer
        GameObject spawnedWire = Instantiate(wireToSpawn, spawnPoint.position, Quaternion.identity);
        SpriteRenderer spawnedWireRenderer = spawnedWire.GetComponent<SpriteRenderer>();
        if (spawnedWireRenderer != null)
        {
            spawnedWireRenderer.enabled = true; // Make the wire visible
        }

        spawnCount++;

        // If all wires are spawned, disable the button
        if (spawnCount >= wirePrefabs.Length || spawnCount >= spawnPoints.Length)
        {
            wirePoints += 10;
            DisableButton();
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
