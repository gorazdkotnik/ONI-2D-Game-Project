using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Position")]
    [SerializeField] float rightBound = 55f;
    [SerializeField] float leftBound = -55f;
    [SerializeField] float yPosition = 10f;
    [SerializeField] GameObject parent;

    [Header("Spawn Settings")]
    [SerializeField] int maxEnemies = 20;
    [SerializeField] float spawnRate = 2f;
    float lastSpawn = 0f;

    [Header("Enemies")]
    [SerializeField] GameObject[] enemyPrefabs;

    void Update()
    {
        SpawnEnemies();
    }

    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject randomEnemy = enemyPrefabs[randomIndex];
        Vector3 spawnPosition = new Vector3(Random.Range(leftBound, rightBound), yPosition, transform.position.z);
        Instantiate(randomEnemy, spawnPosition, Quaternion.identity, parent.transform);
    }

    private void SpawnEnemies()
    {
        if (enemyPrefabs.Length == 0)
        {
            return;
        }

        if (parent.transform.childCount < maxEnemies)
        {
            if (Time.time >= lastSpawn + spawnRate)
            {
                SpawnEnemy();
                lastSpawn = Time.time;
            }
        }
    }
}
