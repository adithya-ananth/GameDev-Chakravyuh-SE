using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public GameObject shipPrefab; // Drag the ship prefab here
    public Transform spawnPoint; // Define the spawn location
    public float spawnInterval = 5f; // Time between ship spawns

    private float timer;
    private void Start()
    {
        SpawnShip();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnShip();
            timer = 0f;
        }
    }

    void SpawnShip()
    {
        Instantiate(shipPrefab, spawnPoint.position, Quaternion.identity);
    }
}
