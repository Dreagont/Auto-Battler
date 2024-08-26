using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blacksmith : MonoBehaviour
{
    public int level = 1;
    public float UpgradeResourceCostReduceRatePerLevel = 0.1f;
    public int maxLevel = 10;

    public float currentExp = 0;
    public float expToNextLevel = 100;
    public float expGrowthRate = 1.5f;

    public int BaseOreUpgradeCost = 200;
    public int BaseGoldUpgradeCost = 100;
    public float ItemLevelMultiplier = 1.5f;
    public int[] slotLevel;
    public MapBonusManager currentMap;
    public GlobalResourceManager GlobalResourceManager;
    public Fighter Fighter;
    public BlacksmithHouse BlacksmithHouse;

    void Start()
    {
        InitItemLevel();
        InitSaveData();
    }

    void Update()
    {
        InitSaveData();
    }

    public void InitSaveData()
    {
        SaveGameManager.data.blacksmithLevel = level;
        SaveGameManager.data.blacksmithCurrentExp = currentExp;
        SaveGameManager.data.blacksmithExpToNextLevel = expToNextLevel;
    }

    public void LoadSaveData()
    {
        level = SaveGameManager.data.blacksmithLevel;
        currentExp = SaveGameManager.data.blacksmithCurrentExp;
        expToNextLevel = SaveGameManager.data.blacksmithCurrentExp;
    }

    public void InitItemLevel()
    {
        for (int i = 0; i < slotLevel.Length; i++)
        {
            slotLevel[i] = 1;
        }
    }

    public void UpgradeItem(int index)
    {
        float levelMulti = CalculateLevelMulti(slotLevel[index]);
        if (slotLevel[index] < BlacksmithHouse.HouseLevel)
        {
            if (CanAffordUpgrade(levelMulti))
            {
                slotLevel[index]++;
                DeductResources(levelMulti);
                Fighter.GetGearStats(Fighter.inventorySystem);
                GainExperience(levelMulti * 10);
                Debug.Log("Upgrade item successful");
            }
        }
        else
        {
            Debug.LogWarning("Upgrade House First");
        }
    }

    private bool CanAffordUpgrade(float levelMulti)
    {
        float reducedGoldCost = CalculateReducedCost(BaseGoldUpgradeCost * levelMulti);
        float reducedOreCost = CalculateReducedCost(BaseOreUpgradeCost * levelMulti);

        return GlobalResourceManager.Gold >= reducedGoldCost
            && GlobalResourceManager.Ores >= reducedOreCost;
    }

    private float CalculateLevelMulti(int itemLevel)
    {
        return (itemLevel > 1 ? (itemLevel - 1) * ItemLevelMultiplier : 1);
    }

    private void DeductResources(float levelMulti)
    {
        float reducedGoldCost = CalculateReducedCost(BaseGoldUpgradeCost * levelMulti);
        float reducedOreCost = CalculateReducedCost(BaseOreUpgradeCost * levelMulti);

        GlobalResourceManager.Ores -= (int)reducedOreCost;
        GlobalResourceManager.Gold -= (int)reducedGoldCost;
    }

    public int GetGoldCost(int level)
    {
        float cost = BaseGoldUpgradeCost * CalculateLevelMulti(level);
        return (int)CalculateReducedCost(cost);
    }

    public int GetOreCost(int level)
    {
        float cost = BaseOreUpgradeCost * CalculateLevelMulti(level);
        return (int)CalculateReducedCost(cost);
    }

    private void GainExperience(float expAmount)
    {
        currentExp += expAmount;
        while (currentExp >= expToNextLevel && level < maxLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        currentExp -= expToNextLevel;
        expToNextLevel *= expGrowthRate;
        Debug.Log($"Blacksmith leveled up to {level}!");
    }

    private float CalculateReducedCost(float baseCost)
    {
        float reduction = (level - 1) * UpgradeResourceCostReduceRatePerLevel;
        return Mathf.Max(baseCost * (1 - reduction), 0);
    }
}