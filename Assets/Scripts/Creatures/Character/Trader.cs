using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    public int sellCount = 1;
    //private float Cooldown = 0f;
    public int SellBonusCount = 1;
    public float SellBonusGold = 1f;
    public int level = 1;
    public float experience = 0f;
    public float baseExperienceToNextLevel = 100f;
    public float experienceMultiplier = 1.5f;

    public GlobalResourceManager GlobalResourceManager;
    private Coroutine autoSellCoroutine;
    public PlayerInventoryHolder InventoryHolder;
    private void Start()
    {
        StartAutoSell();
        InitSaveData();
    }
    void Update()
    {
        CheckLevelUp();
        InitSaveData();
    }
    public void InitSaveData()
    {
        SaveGameManager.data.traderSellBonusCount = SellBonusCount;
        SaveGameManager.data.traderBaseExperienceToNextLevel = baseExperienceToNextLevel;
        SaveGameManager.data.traderSellCount = sellCount;
        SaveGameManager.data.traderExperience = experience;
        SaveGameManager.data.traderSellBonusGold = SellBonusGold;
        SaveGameManager.data.traderLevel = level;
    }

    public void LoadSaveData()
    {
        level = SaveGameManager.data.traderLevel;
        experience = SaveGameManager.data.traderExperience;
        sellCount = SaveGameManager.data.traderSellCount;
        SellBonusGold = SaveGameManager.data.traderSellBonusGold;
        SellBonusCount = SaveGameManager.data.traderSellBonusCount;
        baseExperienceToNextLevel = SaveGameManager.data.traderBaseExperienceToNextLevel;
    }
    public void SellItem(InventoryItemData item, int amount)
    {
        GainExperience(item.sellPrice * SellBonusGold);
        GlobalResourceManager.Gold = (int)(GlobalResourceManager.Gold + item.sellPrice * SellBonusGold * amount);

       
        
    }

    public int GetSellAmount()
    {
        return sellCount + SellBonusCount;
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

        SellBonusGold += 0.1f;
        SellBonusCount += 1;
        Debug.Log("Trader leveled up to level " + level);
    }

    public void GainExperience(float amount)
    {
        experience += amount;
        CheckLevelUp();
    }

    private IEnumerator AutoSellCoroutine()
    {
        while (true)
        {
            AutoSell(InventoryHolder);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnDestroy()
    {
        StopAutoSell();
    }


    public void StartAutoSell()
    {
        if (autoSellCoroutine == null)
        {
            autoSellCoroutine = StartCoroutine(AutoSellCoroutine());
        }
    }

    public void StopAutoSell()
    {
        if (autoSellCoroutine != null)
        {
            StopCoroutine(autoSellCoroutine);
            autoSellCoroutine = null;
        }
    }

    public void AutoSell(PlayerInventoryHolder inventoryHolder)
    {
        InventorySystem inventorySystem = inventoryHolder.SellInventorySystem;
        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            if (inventorySystem.InventorySlots[i].ItemData != null)
            {
                if (GlobalResourceManager.UseAbleEnergy >= 10)
                {
                    GlobalResourceManager.UseAbleEnergy -= 10;
                    if (inventorySystem.InventorySlots[i].StackSize >= GetSellAmount())
                    {
                        SellItem(inventorySystem.InventorySlots[i].ItemData, GetSellAmount());
                        inventorySystem.InventorySlots[i].RemoveFromStack(GetSellAmount());
                    }
                    else
                    {
                        SellItem(inventorySystem.InventorySlots[i].ItemData, inventorySystem.InventorySlots[i].StackSize);
                        inventorySystem.InventorySlots[i].RemoveFromStack(inventorySystem.InventorySlots[i].StackSize);
                    }
                    break;
                }
                
            }
        }
    }
}
