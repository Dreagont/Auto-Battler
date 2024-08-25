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
    public BuiderHouse BuiderHouse;
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
            houseLevelText.text ="LV" + CapacityHouse.HouseLevel.ToString() + "." + CapacityHouse.HouseName;
            goldText.text = (CapacityHouse.BaseGoldUpgradeCost * CalculateLevelMulti(CapacityHouse.HouseLevel, CapacityHouse)).ToString();
            woodText.text = (CapacityHouse.BaseWoodUpgradeCost * CalculateLevelMulti(CapacityHouse.HouseLevel, CapacityHouse)).ToString();
            energyText.text = (CapacityHouse.BaseEnergyUpgradeCost * CalculateLevelMulti(CapacityHouse.HouseLevel, CapacityHouse)).ToString();
        }
        if (BuiderHouse != null)
        {
            houseLevelText.text = "LV" + BuiderHouse.HouseLevel.ToString() + "." + BuiderHouse.HouseName; 
            goldText.text = (BuiderHouse.BaseGoldUpgradeCost * CalculateLevelMultiBuider(BuiderHouse.HouseLevel, BuiderHouse)).ToString();
            woodText.text = (BuiderHouse.BaseWoodUpgradeCost * CalculateLevelMultiBuider(BuiderHouse.HouseLevel, BuiderHouse)).ToString();
            energyText.text = (BuiderHouse.BaseEnergyUpgradeCost * CalculateLevelMultiBuider(BuiderHouse.HouseLevel, BuiderHouse)).ToString();
        }
    }
    private int CalculateLevelMulti(int houseLevel, CapacityHouse house)
    {
        return Mathf.CeilToInt(Mathf.Pow(house.LevelUpMultiply, houseLevel - 1));
    }
    private int CalculateLevelMultiBuider(int houseLevel, BuiderHouse house)
    {
        return Mathf.CeilToInt(Mathf.Pow(house.LevelUpMultiply, houseLevel - 1));
    }
}
