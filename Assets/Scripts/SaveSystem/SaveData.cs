using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SaveData 
{
    //Inventory
    public InventorySaveData playerInventory;
    public InventorySaveData playerEquipment;

    //Character stast
    public float playerBaseMaxHealth;
    public float playerHealth;
    public float playerBaseAttackSpeed;
    public float playerBaseAttackDamage;
    public float playerBaseArmor;
    public float playerBaseRegenAmount;

    public int playerLevel;
    public float playerExperience;
    public float playerBaseExperienceToNextLevel;

    //Global resources
    public int gold;
    public int exchangeableEnergy;
    public int usableEnergy;
    public int maxExchangeableEnergy;
    public int maxUsableEnergy;
    public float elapsedTime;

    //EnemySpawner
    public int spawnerEnemyLevel;
    public int spawnerEnemiesKilledToLevel;
    public int spawnerEnemiesKilledToSpawnTrait;
    public int spawnerCurrentSpawnIndex;
    public bool spawnerStopSpawning;

    public SaveData()
    {
        playerInventory = new InventorySaveData();
        playerEquipment = new InventorySaveData();
    }
}
