using UnityEngine;

public class CampingManager : MonoBehaviour
{
    public GameObject[] campTiles;
    public SpawnerStats[] spawnerStats;
    public EnemySpawner spawner;
    private int oldIndex = 0;

    void Start()
    {
        
    }

    public void SwitchCampTile(int campIndex)
    {
        if (oldIndex != campIndex)
        {
            spawner.StopSpawningAndKillCurrentEnemy();

            for (int i = 0; i < campTiles.Length; i++)
            {
                if(campIndex != i)
                {
                    campTiles [i].SetActive(false);
                }
            }

            campTiles[campIndex].SetActive(true);
            spawner.spawnerStats = spawnerStats[campIndex];

            spawner.ResumeSpawning();

            oldIndex = campIndex;

        }


    }
}
