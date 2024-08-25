using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    public int ExchangeAmount = 10;
    public float ExchangeBonusRate = 1f;
    public int UseAmount = 1;

    private float exchangeCooldown = 0f;

    public int level = 1;
    public float experience = 0f;
    public float baseExperienceToNextLevel = 100f;
    public float experienceMultiplier = 1.5f;

    public MapBonusManager currentMap;
    public GlobalResourceManager GlobalResourceManager;

    private Coroutine autoUseCoroutine;
    public PlayerInventoryHolder InventoryHolder;
    private void Start()
    {
        StartAutoUse();
    }
    void Update()
    {
        CheckLevelUp();
    }

    public void ExchangeEnergy(int bonusAmount)
    {
        if (GlobalResourceManager.ExchangeAbleEnergy > 0)
        {
            if (GlobalResourceManager.UseAbleEnergy < GlobalResourceManager.MaxUseAbleEnergy)
            {
                int sumAmount = ExchangeAmount + bonusAmount;
                int temp = (int)(sumAmount * ExchangeBonusRate);
                exchangeCooldown -= Time.deltaTime;
                if (exchangeCooldown <= 0f)
                {
                    if (GlobalResourceManager.MaxUseAbleEnergy - GlobalResourceManager.UseAbleEnergy > temp)
                    {
                        if (temp <= GlobalResourceManager.ExchangeAbleEnergy)
                        {
                            GlobalResourceManager.ExchangeAbleEnergy -= temp;
                            GlobalResourceManager.UseAbleEnergy += temp;
                        }
                        else
                        {
                            temp = GlobalResourceManager.ExchangeAbleEnergy;
                            GlobalResourceManager.ExchangeAbleEnergy = 0;
                            GlobalResourceManager.UseAbleEnergy += temp;
                        }
                        GainExperience(10);
                        exchangeCooldown = 1f;
                    }
                    
                }
            }
        }
    }

    public void AutoUseItem(InventoryItemData item, int amount)
    {
        GlobalResourceManager.ExchangeAbleEnergy += item.consumeStats.EnergyGain * amount;
        GainExperience(10);
    }

    public int GetUseEnergyOut(InventoryItemData item, int amount)
    {
        return item.consumeStats.EnergyGain * amount;
    }

    private void GainExperience(float amount)
    {
        experience += amount;
        CheckLevelUp();
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

        ExchangeAmount += 5;  
        ExchangeBonusRate += 0.1f;
        UseAmount += 1;

        Debug.Log("Farmer leveled up to level " + level);
    }

    private IEnumerator AutoUseCoroutine()
    {
        while (true)
        {
            AutoUse(InventoryHolder);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnDestroy()
    {
        StopAutoUse();
    }


    public void StartAutoUse()
    {
        if (autoUseCoroutine == null)
        {
            autoUseCoroutine = StartCoroutine(AutoUseCoroutine());
        }
    }

    public void StopAutoUse()
    {
        if (autoUseCoroutine != null)
        {
            StopCoroutine(autoUseCoroutine);
            autoUseCoroutine = null;
        }
    }

    public void AutoUse(PlayerInventoryHolder inventoryHolder)
    {
        InventorySystem inventorySystem = inventoryHolder.UseInventorySystem;
        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            if (inventorySystem.InventorySlots[i].ItemData != null)
            {
                if (GetUseEnergyOut(inventorySystem.InventorySlots[i].ItemData, GetUseAmount()) < GlobalResourceManager.MaxExchangeAbleEnergy - GlobalResourceManager.ExchangeAbleEnergy)
                {
                    if (GlobalResourceManager.UseAbleEnergy >= 10)
                    {
                        GlobalResourceManager.UseAbleEnergy -= 10;
                        if (inventorySystem.InventorySlots[i].StackSize >= GetUseAmount())
                        {
                            AutoUseItem(inventorySystem.InventorySlots[i].ItemData, GetUseAmount());
                            inventorySystem.InventorySlots[i].RemoveFromStack(GetUseAmount());
                        }
                        else
                        {
                            AutoUseItem(inventorySystem.InventorySlots[i].ItemData, inventorySystem.InventorySlots[i].StackSize);
                            inventorySystem.InventorySlots[i].RemoveFromStack(inventorySystem.InventorySlots[i].StackSize);
                        }
                        break;
                    }
                }
            }
        }
    }

    private int GetUseAmount()
    {
       return UseAmount;
    }
}
