using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public GameObject herbivorePrefab;
    public GameObject carnivorePrefab;
    public float spawnRange = 300f;

    private void Start()
    {
        SpawnCarnivore(200);
        SpawnHerbivore(200);
    }

    public void SpawnHerbivore(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Quaternion spawnRotation = GetRandomSpawnRotation();
            Instantiate(herbivorePrefab, spawnPosition, spawnRotation);
        }
    }

    public void SpawnCarnivore(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Quaternion spawnRotation = GetRandomSpawnRotation();
            Instantiate(carnivorePrefab, spawnPosition, spawnRotation);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnRange, spawnRange);
        float randomZ = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPosition = new Vector3(randomX, 2f, randomZ);
        return spawnPosition;
    }

    private Quaternion GetRandomSpawnRotation()
    {
        float randomYRotation = Random.Range(0f, 360f);
        Quaternion spawnRotation = Quaternion.Euler(0f, randomYRotation, 0f);
        return spawnRotation;
    }
}
