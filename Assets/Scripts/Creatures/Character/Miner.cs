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

    }

    void Update()
    {
        CheckLevelUp();
    }

    public void MineOre()
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

        MiningSpeed += 0.1f; 
        MiningQuality += 2; 
        MiningEnergyPerSecond = Mathf.Max(MiningEnergyPerSecond - 1f, 1f); 

        Debug.Log("Miner leveled up to level " + level);
    }

    public void GainExperience(float amount)
    {
        experience += amount;
        CheckLevelUp();
    }

}
