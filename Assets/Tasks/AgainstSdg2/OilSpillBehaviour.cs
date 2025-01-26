using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class OilSpillBehavior : MonoBehaviour
{
    public float spreadSpeed = 0.05f; // Growth speed of the oil spill
    public float maxScale = 3f; // Maximum scale of the spill
    private float pollutionIncrement = 0.1f; // Pollution increment per interval
    public float pollutionInterval = 0.1f; // Time between pollution increments
    public GameObject trashPrefab; // Trash prefab
    public GameObject deadFishPrefab; // Dead fish prefab
    public GameObject[] trashSpawnPoints; // Spawn points
    public int spawnCount = 5; // Number of items to spawn

    private float currentScale = 1f; // Current size of the spill
    private bool itemsSpawned = false; // Ensure items spawn once
    private PollutionManager pollutionManager; // Reference to PollutionManager

    private void OnValidate()
    {
        trashSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Debug.Log($"{trashSpawnPoints.Length} spawn points assigned for {gameObject.name}");
    }

    private void Start()
    {
        pollutionManager = Object.FindFirstObjectByType<PollutionManager>();

        if (pollutionManager == null)
        {
            Debug.LogWarning("PollutionManager not found! Ensure it exists in the scene.");
        }

        // Ensure prefabs are assigned
        if (trashPrefab == null || deadFishPrefab == null)
        {
            Debug.LogError("TrashPrefab or DeadFishPrefab is not assigned! Assign them in the Inspector.");
        }

        // Initialize oil spill size
        transform.localScale = Vector3.one * currentScale;
        StartCoroutine(PollutionUpdateCoroutine());
    }

    private void Update()
    {
        // Spread the oil spill
        if (currentScale < maxScale)
        {
            currentScale += spreadSpeed * Time.deltaTime;
            transform.localScale = Vector3.one * currentScale;
        }

        // Spawn items when the spill is large enough
        if (!itemsSpawned && currentScale >= maxScale / 2f)
        {
            SpawnItems();
            itemsSpawned = true;
        }
    }

    private IEnumerator PollutionUpdateCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(pollutionInterval);

            if (pollutionManager != null)
            {
                pollutionManager.IncreasePollution(.1f);
            }
            else
            {
                Debug.LogWarning("PollutionManager not found! Skipping pollution update.");
            }
        }
    }


    private void SpawnItems()
    {
        if (trashSpawnPoints == null || trashSpawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned for trash or dead fish!");
            return;
        }

        List<GameObject> availableSpawnPoints = new List<GameObject>(trashSpawnPoints);

        int itemsToSpawn = Mathf.Min(spawnCount, availableSpawnPoints.Count);

        for (int i = 0; i < itemsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            GameObject spawnPoint = availableSpawnPoints[randomIndex];
            GameObject prefabToSpawn = Random.value > 0.5f ? trashPrefab : deadFishPrefab;

            if (prefabToSpawn != null)
            {
                Instantiate(prefabToSpawn, spawnPoint.transform.position, Quaternion.identity);
                Debug.Log($"Spawned {prefabToSpawn.name} at {spawnPoint.name}");
            }
            else
            {
                Debug.LogWarning("Prefab to spawn is null! Ensure prefabs are assigned.");
            }

            availableSpawnPoints.RemoveAt(randomIndex);
        }

        Debug.Log($"Spawned {itemsToSpawn} items at random spawn points.");
    }
}
