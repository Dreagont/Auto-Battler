using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    private int currentSpawnIndex = 0;
    private Character character;
    public float spawnDelay = 1f;

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
        }

        currentSpawnIndex = (currentSpawnIndex + 1) % spawnPoints.Length;
    }

    public void OnEnemyKilled()
    {
        StartCoroutine(SpawnEnemyAfterDelay());
    }

    IEnumerator SpawnEnemyAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);  
        SpawnEnemy();
    }
}
