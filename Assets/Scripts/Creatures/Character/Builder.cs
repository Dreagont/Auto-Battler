using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public int level = 1;
    public float UpgradeResourceCostReduceRatePerLevel = 0.1f;
    public int maxLevel = 10;

    public float currentExp = 0;
    public float expToNextLevel = 100;
    public float expGrowthRate = 1.5f;

    public GlobalResourceManager GlobalResourceManager;
    public BuilderHouse buiderHouse;

    private void Start()
    {
        InitSaveData();
    }

    private void Update()
    {
        InitSaveData();
    }

    public void InitSaveData()
    {
        SaveGameManager.data.buiderLevel = level;
        SaveGameManager.data.builderCurrentExp = currentExp;
        SaveGameManager.data.builderExpToNextLevel = expToNextLevel;
    }

    public void LoadSaveData()
    {
        level = SaveGameManager.data.buiderLevel;
        currentExp = SaveGameManager.data.builderCurrentExp;
        expToNextLevel = SaveGameManager.data.builderExpToNextLevel;
    }

    public void LevelUpHouse(CapacityHouse house)
    {
        ResourceType resourceType = house.ResourceType;

        if (house.HouseLevel < BuilderHouseLevel())
        {
            int levelMulti = CalculateLevelMulti(house.HouseLevel, house);

            if (CanAffordUpgrade(levelMulti, house))
            {
                DeductResources(levelMulti, house);
                house.HouseLevel++;
                AdjustMaxValues(resourceType, levelMulti);
                GainExperience(levelMulti * 10);
                Debug.LogWarning(resourceType.ToString());
            }
            else
            {
                Debug.LogWarning("Not Enough Resource");
            }
        }
        else
        {
            Debug.LogWarning("Upgrade Builder House First");
        }
    }

    public void LevelUpAutoHouse(AutomationHouse house)
    {
        if (house.HouseLevel < BuilderHouseLevel())
        {
            int levelMulti = CalculateLevelMulti(house.HouseLevel, house);

            if (CanAffordUpgrade(levelMulti, house))
            {
                DeductResources(levelMulti, house);
                house.OnLevelUp();
                GainExperience(levelMulti * 15); 
                if (house.HouseName == "Farmer House")
                {
                    AdjustMaxValues(ResourceType.UseAbleEnergy, levelMulti);
                    AdjustMaxValues(ResourceType.ExchangeEnergy, levelMulti);
                }
            }
            else
            {
                Debug.LogWarning("Not Enough Resource");
            }
        }
        else
        {
            Debug.LogWarning("Upgrade Builder House First");
        }
    }

    public void LevelUpBuiderHouse(BuilderHouse house)
    {
        int levelMulti = CalculateLevelMultiBuider(house.HouseLevel, house);

        if (CanAffordUpgradeBuilder(levelMulti, house))
        {
            DeductResourcesBuilder(levelMulti, house);
            house.HouseLevel++;
            GainExperience(levelMulti * 20); 
        }
        else
        {
            Debug.LogWarning("Not Enough Resource");
        }
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
        Debug.Log($"Builder leveled up to {level}!");
    }

    private bool CanAffordUpgrade(int levelMulti, CapacityHouse house)
    {
        float reducedGoldCost = CalculateReducedCost(house.BaseGoldUpgradeCost);
        float reducedWoodCost = CalculateReducedCost(house.BaseWoodUpgradeCost);
        float reducedEnergyCost = CalculateReducedCost(house.BaseEnergyUpgradeCost);

        return GlobalResourceManager.Gold >= reducedGoldCost * levelMulti
            && GlobalResourceManager.GetResource(ResourceType.Wood) >= reducedWoodCost * levelMulti
            && GlobalResourceManager.GetResource(ResourceType.UseAbleEnergy) >= reducedEnergyCost * levelMulti;
    }

    private void DeductResources(int levelMulti, CapacityHouse house)
    {
        float reducedGoldCost = CalculateReducedCost(house.BaseGoldUpgradeCost);
        float reducedWoodCost = CalculateReducedCost(house.BaseWoodUpgradeCost);
        float reducedEnergyCost = CalculateReducedCost(house.BaseEnergyUpgradeCost);

        GlobalResourceManager.DeductResource(ResourceType.Wood, (int)(reducedWoodCost * levelMulti));
        GlobalResourceManager.DeductResource(ResourceType.UseAbleEnergy, (int)(reducedEnergyCost * levelMulti));
        GlobalResourceManager.Gold -= (int)(reducedGoldCost * levelMulti);
    }

    private bool CanAffordUpgradeBuilder(int levelMulti, BuilderHouse house)
    {
        float reducedGoldCost = CalculateReducedCost(house.BaseGoldUpgradeCost);
        float reducedWoodCost = CalculateReducedCost(house.BaseWoodUpgradeCost);
        float reducedEnergyCost = CalculateReducedCost(house.BaseEnergyUpgradeCost);

        return GlobalResourceManager.Gold >= reducedGoldCost * levelMulti
            && GlobalResourceManager.GetResource(ResourceType.Wood) >= reducedWoodCost * levelMulti
            && GlobalResourceManager.GetResource(ResourceType.UseAbleEnergy) >= reducedEnergyCost * levelMulti;
    }

    private void DeductResourcesBuilder(int levelMulti, BuilderHouse house)
    {
        float reducedGoldCost = CalculateReducedCost(house.BaseGoldUpgradeCost);
        float reducedWoodCost = CalculateReducedCost(house.BaseWoodUpgradeCost);
        float reducedEnergyCost = CalculateReducedCost(house.BaseEnergyUpgradeCost);

        GlobalResourceManager.DeductResource(ResourceType.Wood, (int)(reducedWoodCost * levelMulti));
        GlobalResourceManager.DeductResource(ResourceType.UseAbleEnergy, (int)(reducedEnergyCost * levelMulti));
        GlobalResourceManager.Gold -= (int)(reducedGoldCost * levelMulti);
    }

    private void AdjustMaxValues(ResourceType resourceType, int levelMulti)
    {
        GlobalResourceManager.IncreaseMaxValue(resourceType, 500 * levelMulti);
        Debug.LogWarning("adjust" + resourceType.ToString());
    }

    private int CalculateLevelMulti(int houseLevel, CapacityHouse house)
    {
        return Mathf.CeilToInt(Mathf.Pow(house.LevelUpMultiply, houseLevel - 1));
    }

    private int CalculateLevelMultiBuider(int houseLevel, BuilderHouse house)
    {
        return Mathf.CeilToInt(Mathf.Pow(house.LevelUpMultiply, houseLevel - 1));
    }

    private int BuilderHouseLevel()
    {
        return buiderHouse.HouseLevel;
    }

    public float CalculateReducedCost(float baseCost)
    {
        float reduction = (level - 1) * UpgradeResourceCostReduceRatePerLevel;
        return Mathf.Max(baseCost * (1 - reduction), 0);
    }
}

public enum ResourceType
{
    None,
    Wood,
    UseAbleEnergy,
    ExchangeEnergy,
    Ore,
    Gold
}