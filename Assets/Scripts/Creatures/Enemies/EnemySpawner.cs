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
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points set for EnemySpawner.");
            return;
        }

        Transform spawnPoint = spawnPoints[currentSpawnIndex];
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyObj = Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoint.position, spawnPoint.rotation);
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        if (enemy != null)
        {
            if (character != null)
            {
                character.AddEnemy(enemy);
            }
            // Set the level of the newly spawned enemy
            enemy.SetLevel(enemyLevel);
        }

        currentSpawnIndex = (currentSpawnIndex + 1) % spawnPoints.Length;
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;

        if (enemiesKilled >= 20)
        {
            enemyLevel++;
            enemiesKilled = 0; // Reset the kill counter
        }

        StartCoroutine(SpawnEnemyAfterDelay());
    }

    IEnumerator SpawnEnemyAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnEnemy();
    }
}
