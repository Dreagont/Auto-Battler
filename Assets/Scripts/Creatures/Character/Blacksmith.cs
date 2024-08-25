using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blacksmith : MonoBehaviour
{
    //private float Cooldown = 0f;

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
    }

    void Update()
    {
        
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
                Debug.Log("Upgrade item succ");
            }
        } else
        {
            Debug.LogWarning("Upgrade House First");
        }
        
    }

    private bool CanAffordUpgrade(float levelMulti)
    {
        return GlobalResourceManager.Gold >= BaseGoldUpgradeCost * levelMulti
            && GlobalResourceManager.Ores >= BaseOreUpgradeCost * levelMulti;
    }
    private float CalculateLevelMulti(int itemLevel)
    {
        return (itemLevel > 1 ? (itemLevel - 1) * 1.5f : 1);
    }
    private void DeductResources(float levelMulti)
    {

        GlobalResourceManager.Ores -= (int)(BaseGoldUpgradeCost * levelMulti);
        GlobalResourceManager.Gold -= (int)(BaseGoldUpgradeCost * levelMulti);
    }

    public int GetGoldCost(int level)
    {
        return (int)(BaseGoldUpgradeCost * CalculateLevelMulti(level));
    }
    public int GetOreCost(int level)
    {
        return (int)(BaseOreUpgradeCost * CalculateLevelMulti(level));
    }
}
