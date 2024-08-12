using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    private int currentSpawnIndex = 0;
    private Character character;
    public float spawnDelay = 1f;

    private int enemiesKilled = 0;
    private int enemyLevel = 1;
    private Enemy currentEnemy;
    private bool stopSpawning = false;

    void Start()
    {
        character = FindObjectOfType<Character>();
        if (character == null)
        {
            Debug.LogError("Character not found in the scene.");
            return;
        }
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (stopSpawning) return;

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points set for EnemySpawner.");
            return;
        }

        Transform spawnPoint = spawnPoints[currentSpawnIndex];
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyObj = Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoint.position, spawnPoint.rotation);
        currentEnemy = enemyObj.GetComponent<Enemy>();

        if (currentEnemy != null)
        {
            if (character != null)
            {
                character.AddEnemy(currentEnemy);
            }
            currentEnemy.SetLevel(enemyLevel);
        }

        currentSpawnIndex = (currentSpawnIndex + 1) % spawnPoints.Length;
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;

        if (enemiesKilled >= 20)
        {
            enemyLevel++;
            enemiesKilled = 0; 
        }

        StartCoroutine(SpawnEnemyAfterDelay());
    }

    IEnumerator SpawnEnemyAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
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

    void Update()
    {
        if (character != null && character.IsDead())
        {
            StopSpawningAndKillCurrentEnemy();
        }

        if (stopSpawning && character != null && character.IsHealthFull())
        {
            CheckPlayerHealthAndResumeSpawning();
        }
    }
}
