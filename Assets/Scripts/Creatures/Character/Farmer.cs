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

    void Start()
    {

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

    public void AutoUseItem(InventoryItemData item, int amount)
    {
        if (GlobalResourceManager.UseAbleEnergy >= 10)
        {
            GlobalResourceManager.UseAbleEnergy += item.consumeStats.EnergyGain * amount;
            GlobalResourceManager.UseAbleEnergy -= 10;
            GainExperience(10); 
        }
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
}
