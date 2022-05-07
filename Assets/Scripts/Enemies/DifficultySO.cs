using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Difficulty", fileName = "Difficulty Level")]
public class DifficultySO : ScriptableObject
{
  [Header("Spawn Settings")]
  [SerializeField] int maxEnemies = 20;
  [SerializeField] int maxBosses = 1;
  [SerializeField] float spawnRate = 2f;
  [SerializeField] float bossSpawnRate = 10f;

  public int GetMaxEnemies() { return maxEnemies; }
  public int GetMaxBosses() { return maxBosses; }
  public float GetSpawnRate() { return spawnRate; }
  public float GetBossSpawnRate() { return bossSpawnRate; }

}
