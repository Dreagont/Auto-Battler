using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Miner : MonoBehaviour
{
    //private Characters myCharacter = Characters.Miner;

    public float MiningSpeed = 1.0f; 
    public int MiningQuality = 5; 
    public float MiningEnergyPerSecond = 10f; 

    public int level = 1;
    public float experience = 0f;
    public float baseExperienceToNextLevel = 100f;
    public float experienceMultiplier = 1.5f;

    private float Cooldown;

    public MapBonusManager currentMap;

    public GlobalResourceManager GlobalResourceManager;

    void Start()
    {
        InitSaveData();
    }

    void Update()
    {
        CheckLevelUp();
        InitSaveData();
    }

    public void InitSaveData()
    {
        SaveGameManager.data.minerLevel = level;
        SaveGameManager.data.minerExperience = experience;
        SaveGameManager.data.minerBaseExperienceToNextLevel = baseExperienceToNextLevel;
        SaveGameManager.data.MiningEnergyPerSecond = MiningEnergyPerSecond;
        SaveGameManager.data.MiningQuality = MiningQuality;
        SaveGameManager.data.MiningSpeed = MiningSpeed;
    }

    public void LoadSaveData()
    {
        level = SaveGameManager.data.minerLevel;
        experience = SaveGameManager.data.minerExperience;
        baseExperienceToNextLevel = SaveGameManager.data.minerBaseExperienceToNextLevel;
        MiningSpeed = SaveGameManager.data.MiningSpeed;
        MiningQuality = SaveGameManager.data.MiningQuality;
        MiningEnergyPerSecond = SaveGameManager.data.MiningEnergyPerSecond;
        StatsMultiplier(level);
    }

    public void MineOre()
    {
        if (GlobalResourceManager.Ores < GlobalResourceManager.MaxOres)
        {
            Cooldown -= Time.deltaTime;
            if (Cooldown <= 0f)
            {
                if (GlobalResourceManager.UseAbleEnergy >= currentMap.minerArenaHarvestBonus.EnergyCost)
                {
                    GlobalResourceManager.Ores += MiningQuality + currentMap.minerArenaHarvestBonus.arenaQualityBonus;
                    GlobalResourceManager.UseAbleEnergy -= currentMap.minerArenaHarvestBonus.EnergyCost;
                    Cooldown = 1f / (MiningSpeed + currentMap.minerArenaHarvestBonus.arenaSpeedBonus);
                    GainExperience(MiningQuality + currentMap.minerArenaHarvestBonus.arenaQualityBonus);
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

        StatsMultiplier(level);

        Debug.Log("Miner leveled up to level " + level);
    }

    private void StatsMultiplier(int currentLevel)
    {
        MiningSpeed = 1;
        MiningQuality = 5;
        MiningEnergyPerSecond = 10;

        MiningSpeed += currentLevel * 0.1f;
        MiningQuality += currentLevel * 2;
        MiningEnergyPerSecond = Mathf.Max(MiningEnergyPerSecond - currentLevel, 1f);
    }
    public void GainExperience(float amount)
    {
        experience += amount;
        CheckLevelUp();
    }

}
