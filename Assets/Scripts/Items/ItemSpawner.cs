using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Position")]
    [SerializeField] float rightBound = 55f;
    [SerializeField] float leftBound = -55f;
    [SerializeField] float yPosition = 10f;
    [SerializeField] GameObject parentItems;

    [Header("Spawn Settings")]
    [SerializeField] int maxItems = 20;
    [SerializeField] float spawnRate = 1f;
    [SerializeField] float minScale = 5f;
    [SerializeField] float maxScale = 10f;
    float lastSpawn = 0f;

    [Header("Items")]
    [SerializeField] GameObject[] itemPrefabs;



    void Update()
    {
        SpawnItems();
    }

    private void SpawnItem()
    {
        int randomIndex = Random.Range(0, itemPrefabs.Length);

        GameObject randomItem = itemPrefabs[randomIndex];

        if (randomItem.tag == "Gem")
        {
            randomItem = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
        }

        Vector3 spawnPosition = new Vector3(Random.Range(leftBound, rightBound), yPosition, transform.position.z);
        GameObject newGameObject = Instantiate(randomItem, spawnPosition, Quaternion.identity, parentItems.transform);

        float scale = Random.Range(minScale, maxScale);
        newGameObject.transform.localScale = new Vector3(scale, scale, 1f);
    }

    private void SpawnItems()
    {
        if (itemPrefabs.Length == 0)
        {
            return;
        }

        if (parentItems.transform.childCount < maxItems) {
            if (Time.time - lastSpawn > spawnRate)
            {
                SpawnItem();
                lastSpawn = Time.time;
            }
        }
    }
}

