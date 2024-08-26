using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderHouse : MonoBehaviour
{
    public int HouseLevel;
    public int BaseWoodUpgradeCost = 300;
    public int BaseEnergyUpgradeCost = 200;
    public int BaseGoldUpgradeCost = 200;
    public string HouseName;

    public float LevelUpMultiply = 1.5f;

    public MapBonusManager currentMap;

    public GlobalResourceManager GlobalResourceManager;

    void Start()
    {
        InitSaveData();
    }

    void Update()
    {
        InitSaveData();
    }

    public void InitSaveData()
    {
        if (HouseName == "Builder House")
        {
            SaveGameManager.data.BuilderHouseLevel = HouseLevel;
        }
        else
        {
            SaveGameManager.data.BlacksmithHouseLevel = HouseLevel;

        }
    }

    public void LoadSaveData()
    {
        if (HouseName == "Builder House")
        {
            HouseLevel = SaveGameManager.data.BuilderHouseLevel;
        } else
        {
            HouseLevel = SaveGameManager.data.BlacksmithHouseLevel;
        }
    }


}
