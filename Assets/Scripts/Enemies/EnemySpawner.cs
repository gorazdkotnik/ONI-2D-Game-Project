using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Position")]
    [SerializeField] float rightBound = 55f;
    [SerializeField] float leftBound = -55f;
    [SerializeField] float yPosition = 10f;
    [SerializeField] GameObject parentEnemies;
    [SerializeField] GameObject parentBosses;

    [Header("Spawn Settings")]
    [SerializeField] int maxEnemies = 20;
    [SerializeField] int maxBosses = 1;
    [SerializeField] float spawnRate = 2f;
    [SerializeField] float bossSpawnRate = 10f;
    float lastSpawn = 0f;
    float lastBossSpawn = 0f;

    [Header("Enemies")]
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject[] bossesPrefabs;

    void Start() {
        SpawnEnemiesAtStart();
    }

    void Update()
    {
        SpawnEnemies();
        SpawnBosses();
    }

    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject randomEnemy = enemyPrefabs[randomIndex];
        Vector3 spawnPosition = new Vector3(Random.Range(leftBound, rightBound), yPosition, transform.position.z);
        Instantiate(randomEnemy, spawnPosition, Quaternion.identity, parentEnemies.transform);
    }

    void SpawnBoss() {
        int randomIndex = Random.Range(0, bossesPrefabs.Length);
        GameObject randomBoss = bossesPrefabs[randomIndex];
        Vector3 spawnPosition = new Vector3(Random.Range(leftBound, rightBound), yPosition, transform.position.z);
        Instantiate(randomBoss, spawnPosition, Quaternion.identity, parentBosses.transform);
    }

    void SpawnEnemiesAtStart() {
        lastSpawn = Time.time;
        for (int i = 0; i < maxEnemies / 2; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemies()
    {
        if (enemyPrefabs.Length == 0)
        {
            return;
        }

        if (parentEnemies.transform.childCount < maxEnemies)
        {
            if (Time.time >= lastSpawn + spawnRate)
            {
                SpawnEnemy();
                lastSpawn = Time.time;
            }
        }
    }

    void SpawnBosses() {
        if (bossesPrefabs.Length == 0)
        {
            return;
        }

        if (parentBosses.transform.childCount < maxBosses)
        {
            if (Time.time >= lastBossSpawn + bossSpawnRate)
            {
                SpawnBoss();
                lastBossSpawn = Time.time;
            }
        }
    }
}
