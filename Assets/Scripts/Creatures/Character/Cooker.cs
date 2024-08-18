using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cooker : MonoBehaviour
{
    public int ExchangeAmount = 10;
    public float ExchangeBonusRate = 1f;

    private float Cooldown = 0f;

    public MapBonusManager currentMap;

    public GlobalResourceManager GlobalResourceManager;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ExchangeEnergy(int bonusAmount)
    {
        float bonusRate = ExchangeBonusRate;
        if (GlobalResourceManager.ExchangeAbleEnergy <= 0)
        {
            GlobalResourceManager.ExchangeAbleEnergy = 0;
        }
        else
        {
            if (GlobalResourceManager.UseAbleEnergy < GlobalResourceManager.MaxUseAbleEnergy)
            {
                int SumAmount = ExchangeAmount + bonusAmount;
                int temp = (int)(SumAmount * bonusRate);
                Cooldown -= Time.deltaTime;
                if (Cooldown <= 0f)
                {
                    if (temp <= GlobalResourceManager.ExchangeAbleEnergy)
                    {
                        GlobalResourceManager.ExchangeAbleEnergy = GlobalResourceManager.ExchangeAbleEnergy - temp;
                        GlobalResourceManager.UseAbleEnergy = GlobalResourceManager.UseAbleEnergy + temp;
                    }
                    else
                    {
                        temp = GlobalResourceManager.ExchangeAbleEnergy;
                        GlobalResourceManager.ExchangeAbleEnergy = GlobalResourceManager.ExchangeAbleEnergy - temp;
                        GlobalResourceManager.UseAbleEnergy = GlobalResourceManager.UseAbleEnergy + temp;
                    }
                    Cooldown = 1f;
                }
            }
        }
    }
}
