using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map System/Spawner Stats")]
public class SpawnerStats : ScriptableObject
{
    public string spawnerName;
    public GameObject[] enemyPrefabs;
    public float spawnDelay = 1f;
    public int enemyLevel = 1;

    public int levelUpCount = 10;
    public float eliteSpawnRate = 0.1f; 
    public float bossSpawnRate = 0.01f;
}
