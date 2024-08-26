using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HouseUpgradeUI : MonoBehaviour
{
    public TMP_Text houseLevelText;
    public TMP_Text goldText;
    public TMP_Text woodText;
    public TMP_Text energyText;
    public CapacityHouse CapacityHouse;
    public BuilderHouse BuiderHouse;
    public Builder Builder;

    void Start()
    {
        
    }

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (CapacityHouse != null)
        {
            houseLevelText.text = "LV" + CapacityHouse.HouseLevel.ToString() + "." + CapacityHouse.HouseName;
            
            int levelMulti = CalculateLevelMulti(CapacityHouse.HouseLevel, CapacityHouse);
            float reducedGoldCost = CalculateReducedCost(CapacityHouse.BaseGoldUpgradeCost * levelMulti);
            float reducedWoodCost = CalculateReducedCost(CapacityHouse.BaseWoodUpgradeCost * levelMulti);
            float reducedEnergyCost = CalculateReducedCost(CapacityHouse.BaseEnergyUpgradeCost * levelMulti);

            goldText.text = reducedGoldCost.ToString("F0");
            woodText.text = reducedWoodCost.ToString("F0");
            energyText.text = reducedEnergyCost.ToString("F0");
        }
        if (BuiderHouse != null)
        {
            houseLevelText.text = "LV" + BuiderHouse.HouseLevel.ToString() + "." + BuiderHouse.HouseName; 
            
            int levelMulti = CalculateLevelMultiBuider(BuiderHouse.HouseLevel, BuiderHouse);
            float reducedGoldCost = CalculateReducedCost(BuiderHouse.BaseGoldUpgradeCost * levelMulti);
            float reducedWoodCost = CalculateReducedCost(BuiderHouse.BaseWoodUpgradeCost * levelMulti);
            float reducedEnergyCost = CalculateReducedCost(BuiderHouse.BaseEnergyUpgradeCost * levelMulti);

            goldText.text = reducedGoldCost.ToString("F0");
            woodText.text = reducedWoodCost.ToString("F0");
            energyText.text = reducedEnergyCost.ToString("F0");
        }
    }

    private int CalculateLevelMulti(int houseLevel, CapacityHouse house)
    {
        return Mathf.CeilToInt(Mathf.Pow(house.LevelUpMultiply, houseLevel - 1));
    }

    private int CalculateLevelMultiBuider(int houseLevel, BuilderHouse house)
    {
        return Mathf.CeilToInt(Mathf.Pow(house.LevelUpMultiply, houseLevel - 1));
    }

    private float CalculateReducedCost(float baseCost)
    {
        if (Builder != null)
        {
            float reduction = (Builder.level - 1) * Builder.UpgradeResourceCostReduceRatePerLevel;
            return Mathf.Max(baseCost * (1 - reduction), 0);
        }
        return baseCost;
    }
}