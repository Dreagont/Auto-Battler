using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public SpawnerStats spawnerStats; 

    private int currentSpawnIndex = 0;
    private Fighter character;
    private Enemy currentEnemy;
    private bool stopSpawning = false;
    public Transform[] spawnPoints;

    private int _enemyLevel = 1;
    private int enemiesKilledToLevel = 0;
    void Start()
    {
        if (spawnerStats == null)
        {
            Debug.LogError("SpawnerStats not assigned.");
            return;
        }

        character = FindObjectOfType<Fighter>();
        if (character == null)
        {
            Debug.LogError("Character not found in the scene.");
            return;
        }

        SaveSpawnerData();
        StartCoroutine(SpawnEnemyAfterDelay());
    }

    void Update()
    {
        SaveSpawnerData();
        if (character != null)
        {
            if (character.IsDead())
            {
                StopSpawningAndKillCurrentEnemy();
            }

            if (stopSpawning && character.IsHealthFull())
            {
                CheckPlayerHealthAndResumeSpawning();
            }
        }
    }
    public void SaveSpawnerData()
    {
        SaveGameManager.data.spawnerEnemyLevel = _enemyLevel;
        SaveGameManager.data.spawnerEnemiesKilledToLevel = spawnerStats.levelUpCount;
        SaveGameManager.data.eliteSpawnRate = spawnerStats.eliteSpawnRate;
        SaveGameManager.data.bossSpawnRate = spawnerStats.bossSpawnRate;
        SaveGameManager.data.spawnerCurrentSpawnIndex = currentSpawnIndex;
        SaveGameManager.data.spawnerStopSpawning = stopSpawning;
    }
    public void LoadSpawnerData(SaveData data)
    {
        _enemyLevel = data.spawnerEnemyLevel;
        spawnerStats.levelUpCount = data.spawnerEnemiesKilledToLevel;
        spawnerStats.eliteSpawnRate = data.eliteSpawnRate;
        spawnerStats.bossSpawnRate = data.bossSpawnRate;
        currentSpawnIndex = data.spawnerCurrentSpawnIndex;
        stopSpawning = data.spawnerStopSpawning;
    }

    void SpawnEnemy()
    {
        if (stopSpawning) return;

        if (spawnerStats.enemyPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogError("No enemy prefabs or spawn points set.");
            return;
        }

        Transform spawnPoint = spawnPoints[currentSpawnIndex];
        int randomEnemyIndex = Random.Range(0, spawnerStats.enemyPrefabs.Length);

        GameObject enemyObj = Instantiate(spawnerStats.enemyPrefabs[randomEnemyIndex], spawnPoint.position, spawnPoint.rotation);
        currentEnemy = enemyObj.GetComponent<Enemy>();

        currentEnemy.enemyTypeData.level = _enemyLevel;
        currentEnemy.enemyTypeData.enemyTraits = EnemyTraits.Normal;

        if (currentEnemy != null)
        {
            currentEnemy.enemyTypeData.level = spawnerStats.enemyLevel;
            currentEnemy.enemyTypeData.enemyTraits = DetermineEnemyTrait();
            currentEnemy.SetLevel(_enemyLevel);

            if (character != null)
            {
                character.AddEnemy(currentEnemy);
            }
        }

        currentSpawnIndex = (currentSpawnIndex + 1) % spawnPoints.Length;
    }

    private EnemyTraits DetermineEnemyTrait()
    {
        float spawnChance = Random.value;

        if (spawnChance <= spawnerStats.bossSpawnRate)
        {
            return EnemyTraits.Boss;
        }
        else if (spawnChance <= spawnerStats.eliteSpawnRate)
        {
            return EnemyTraits.Elite;
        }
        else
        {
            return EnemyTraits.Normal;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesKilledToLevel++;

        if (enemiesKilledToLevel >=spawnerStats.levelUpCount)
        {
            _enemyLevel++;
            enemiesKilledToLevel = 0;
        }

        StartCoroutine(SpawnEnemyAfterDelay());
    }

    IEnumerator SpawnEnemyAfterDelay()
    {
        yield return new WaitForSeconds(spawnerStats.spawnDelay);
        SpawnEnemy();
    }

    public void StopSpawningAndKillCurrentEnemy()
    {
        stopSpawning = true;

        if (currentEnemy != null)
        {
            Destroy(currentEnemy.gameObject);
            currentEnemy = null;
        }
    }

    public void CheckPlayerHealthAndResumeSpawning()
    {
        if (character != null && character.IsHealthFull())
        {
            stopSpawning = false;
            SpawnEnemy();
        }
    }

    public void ResumeSpawning()
    {
        stopSpawning = false;
        SpawnEnemy();
    }
}
