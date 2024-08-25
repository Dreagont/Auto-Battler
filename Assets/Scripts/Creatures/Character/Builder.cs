using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
   
    public GlobalResourceManager GlobalResourceManager;
    public BuiderHouse buiderHouse;

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
                Debug.LogWarning(resourceType.ToString());
            } else
            {
                Debug.LogWarning("Not Enough Resousce");
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
                if (house.HouseName == "Farmer House")
                {
                    AdjustMaxValues(ResourceType.UseAbleEnergy, levelMulti);
                    AdjustMaxValues(ResourceType.ExchangeEnergy, levelMulti);
                }
            }
            else
            {
                Debug.LogWarning("Not Enough Resousce");
            }
        }
        else
        {
            Debug.LogWarning("Upgrade Builder House First");
        }
    }

    public void LevelUpBuiderHouse(BuiderHouse house)
    {
        int levelMulti = CalculateLevelMultiBuider(house.HouseLevel, house);

        if (CanAffordUpgradeBuilder(levelMulti, house))
        {
            DeductResourcesBuilder(levelMulti, house);
            house.HouseLevel++;
        } else
        {
            Debug.LogWarning("Not Enough Resousce");

        }
    }

    private bool CanAffordUpgrade(int levelMulti, CapacityHouse house)
    {
        return GlobalResourceManager.Gold >= house.BaseGoldUpgradeCost * levelMulti
            && GlobalResourceManager.GetResource(ResourceType.Wood) >= house.BaseWoodUpgradeCost * levelMulti
            && GlobalResourceManager.GetResource(ResourceType.UseAbleEnergy) >= house.BaseEnergyUpgradeCost * levelMulti;
    }

    private void DeductResources(int levelMulti, CapacityHouse house)
    {
        GlobalResourceManager.DeductResource(ResourceType.Wood, house.BaseWoodUpgradeCost * levelMulti);
        GlobalResourceManager.DeductResource(ResourceType.UseAbleEnergy, house.BaseEnergyUpgradeCost * levelMulti);
        GlobalResourceManager.Gold -= house.BaseGoldUpgradeCost * levelMulti;
    }

    private bool CanAffordUpgradeBuilder(int levelMulti, BuiderHouse house)
    {
        return GlobalResourceManager.Gold >= house.BaseGoldUpgradeCost * levelMulti
            && GlobalResourceManager.GetResource(ResourceType.Wood) >= house.BaseWoodUpgradeCost * levelMulti
            && GlobalResourceManager.GetResource(ResourceType.UseAbleEnergy) >= house.BaseEnergyUpgradeCost * levelMulti;
    }

    private void DeductResourcesBuilder(int levelMulti, BuiderHouse house)
    {
        GlobalResourceManager.DeductResource(ResourceType.Wood, house.BaseWoodUpgradeCost * levelMulti);
        GlobalResourceManager.DeductResource(ResourceType.UseAbleEnergy, house.BaseEnergyUpgradeCost * levelMulti);
        GlobalResourceManager.Gold -= house.BaseGoldUpgradeCost * levelMulti;
    }
    private void AdjustMaxValues(ResourceType resourceType, int levelMulti)
    {
        GlobalResourceManager.IncreaseMaxValue(resourceType, 500 * levelMulti);
    }

    private int CalculateLevelMulti(int houseLevel, CapacityHouse house)
    {
        return Mathf.CeilToInt(Mathf.Pow(house.LevelUpMultiply, houseLevel - 1));
    }
    private int CalculateLevelMultiBuider(int houseLevel, BuiderHouse house)
    {
        return Mathf.CeilToInt(Mathf.Pow(house.LevelUpMultiply, houseLevel - 1));
    }
    private int BuilderHouseLevel()
    {
        return buiderHouse.HouseLevel;
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
