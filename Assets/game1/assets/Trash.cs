using UnityEngine;

public class Trash : MonoBehaviour
{
    public GameObject[] trashPrefabs; // Array of trash prefabs
    public float spawnInterval = 2f; // Time between spawns

    [System.Serializable]
    public struct SpawnZone
    {
        public Vector2 center; // Center of the spawn zone
        public Vector2 size;   // Size of the spawn zone
    }

    public SpawnZone[] spawnZones; // Array of spawn zones

    void Start()
    {
        InvokeRepeating("SpawnTrash", 1f, spawnInterval);
    }

    void SpawnTrash()
    {
        // Select a random spawn zone
        int zoneIndex = Random.Range(0, spawnZones.Length);
        SpawnZone selectedZone = spawnZones[zoneIndex];

        // Calculate a random position within the selected zone
        Vector2 spawnPosition = new Vector2(
            Random.Range(selectedZone.center.x - selectedZone.size.x / 2, selectedZone.center.x + selectedZone.size.x / 2),
            Random.Range(selectedZone.center.y - selectedZone.size.y / 2, selectedZone.center.y + selectedZone.size.y / 2)
        );

        // Select a random trash prefab
        int randomIndex = Random.Range(0, trashPrefabs.Length);

        // Instantiate the trash at the calculated position
        Instantiate(trashPrefabs[randomIndex], spawnPosition, Quaternion.identity);
    }
}
