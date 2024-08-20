using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lumberjack : MonoBehaviour
{
    //private Characters myCharacter = Characters.Lumberjack;

    public float ChoppingSpeed = 1.0f; 
    public int ChoppingQuality = 5; 
    public int ChoppingEnergyPerSecond = 10; 

    public int level = 1;
    public float experience = 0f;
    public float baseExperienceToNextLevel = 100f;
    public float experienceMultiplier = 1.5f;

    private float Cooldown;

    public MapBonusManager currentMap;

    public GlobalResourceManager GlobalResourceManager;

    void Start()
    {

    }

    void Update()
    {
        CheckLevelUp();
    }
    
    public void ChopWood()
    {
        if (GlobalResourceManager.Woods < GlobalResourceManager.MaxWoods)
        {
            Cooldown -= Time.deltaTime;
            if (Cooldown <= 0f)
            {
                if (GlobalResourceManager.UseAbleEnergy >= currentMap.lumberArenaHarvestBonus.EnergyCost)
                {
                    GlobalResourceManager.Woods += ChoppingQuality + currentMap.lumberArenaHarvestBonus.arenaQualityBonus;
                    GlobalResourceManager.UseAbleEnergy -= currentMap.lumberArenaHarvestBonus.EnergyCost;
                    Cooldown = 1f / (ChoppingSpeed + currentMap.lumberArenaHarvestBonus.arenaSpeedBonus);
                    GainExperience(ChoppingQuality + currentMap.lumberArenaHarvestBonus.arenaQualityBonus);

                }
            }
        } 
    }

    private void CheckLevelUp()
    {
        if (experience >= baseExperienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        experience -= baseExperienceToNextLevel;
        baseExperienceToNextLevel *= experienceMultiplier;

        ChoppingSpeed += 0.1f;
        ChoppingQuality += 2; 
        ChoppingEnergyPerSecond = Mathf.Max(ChoppingEnergyPerSecond - 1, 1); 

        Debug.Log("Lumberjack leveled up to level " + level);
    }

    public void GainExperience(float amount)
    {
        experience += amount;
        CheckLevelUp();
    }
}
