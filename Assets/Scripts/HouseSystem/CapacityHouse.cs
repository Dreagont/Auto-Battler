using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacityHouse : MonoBehaviour
{
    public int HouseLevel;
    public int BaseWoodUpgradeCost = 300;
    public int BaseEnergyUpgradeCost = 50;
    public int BaseGoldUpgradeCost = 200;
    public string HouseName;

    public float LevelUpMultiply = 1.5f;

    public ResourceType ResourceType; 
    public GlobalResourceManager GlobalResourceManager;
    void Start()
    {
        InitSaveData();

    }

    // Update is called once per frame
    void Update()
    {
        InitSaveData();

    }

    public void InitSaveData()
    {
        if (HouseName == "Farmer House")
        {
            SaveGameManager.data.FarmerHouseLevel = HouseLevel;
        }
        if (HouseName == "Trader House")
        {
            SaveGameManager.data.TraderHouseLevel = HouseLevel;
        }
        if (HouseName == "Lummer House")
        {
            SaveGameManager.data.LummerHouseLevel = HouseLevel;
        }
        if (HouseName == "Miner House")
        {
            SaveGameManager.data.MinerHouseLevel = HouseLevel;
        }
    }

    public void LoadSaveData()
    {
        if (HouseName == "Farmer House")
        {
            HouseLevel = SaveGameManager.data.FarmerHouseLevel;
        }
        if (HouseName == "Trader House")
        {
            HouseLevel = SaveGameManager.data.TraderHouseLevel;
        }
        if (HouseName == "Lummer House")
        {
            HouseLevel = SaveGameManager.data.LummerHouseLevel;
        }
        if (HouseName == "Miner House")
        {
            HouseLevel = SaveGameManager.data.MinerHouseLevel;
        }
    }
}
