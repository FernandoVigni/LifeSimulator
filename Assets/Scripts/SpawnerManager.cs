using UnityEngine;
using System.Collections.Generic;

public class SpawnerManager : MonoBehaviour
{
    public GameObject herbivorePrefab;
    public GameObject carnivorePrefab;
    public float spawnRange = 300f;
    public int carnivores;
    public int herbivores;

    public List<GameObject> herbivoreList;
    public List<GameObject> carnivoreList;

    private void Start()
    {
        herbivoreList.Clear();
        SpawnCarnivore(carnivores);
        SpawnHerbivore(herbivores);
    }

    public void SpawnHerbivore(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Quaternion spawnRotation = GetRandomSpawnRotation();
            GameObject herbivore = Instantiate(herbivorePrefab, spawnPosition, spawnRotation);
            herbivoreList.Add(herbivore);
        }
    }

    public void SpawnCarnivore(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Quaternion spawnRotation = GetRandomSpawnRotation();
            GameObject carnivore = Instantiate(carnivorePrefab, spawnPosition, spawnRotation);
            carnivoreList.Add(carnivore);
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
