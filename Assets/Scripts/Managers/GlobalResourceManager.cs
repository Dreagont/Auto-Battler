using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalResourceManager : MonoBehaviour
{
    public int Gold = 0;
    public int ExchangeAbleEnergy = 1000;
    public int UseAbleEnergy;

    public int MaxExchangeAbleEnergy = 2000;
    public int MaxUseAbleEnergy = 100;

    public int ExchangeAmount = 10;

    public TMP_Text GoldText;
    public TMP_Text ExchangeAbleEnergyText;
    public TMP_Text UseAbleEnergyText;
    public TMP_Text MaxExchangeAbleEnergyText;
    public TMP_Text MaxUseAbleEnergyText;

    private float Cooldown = 0f;

    public BarManager UseBar;
    public BarManager ExchangBar;

    private void Update()
    {
        UseBar.UpdateBar(UseAbleEnergy,MaxUseAbleEnergy);
        ExchangBar.UpdateBar(ExchangeAbleEnergy,MaxExchangeAbleEnergy);

        GoldText.text = ReuseMethod.FormatNumber(Gold);
        ExchangeAbleEnergyText.text = ReuseMethod.FormatNumber(ExchangeAbleEnergy);
        UseAbleEnergyText.text= ReuseMethod.FormatNumber(UseAbleEnergy);
        MaxExchangeAbleEnergyText.text = ReuseMethod.FormatNumber(MaxExchangeAbleEnergy);
        MaxUseAbleEnergyText.text = ReuseMethod.FormatNumber(MaxUseAbleEnergy);


        if (UseAbleEnergy > MaxUseAbleEnergy)
        {
            UseAbleEnergy = MaxUseAbleEnergy;
        }

        if (ExchangeAbleEnergy > MaxExchangeAbleEnergy)
        {
            ExchangeAbleEnergy = MaxExchangeAbleEnergy;
        }

        if (ExchangeAbleEnergy <= 0)
        {
            ExchangeAbleEnergy = 0;
        } else
        {
            ExchangeEnergy(0, 1);
        }
    }

    public void ExchangeEnergy(int bonusAmount, float bonusRate)
    {
        if (UseAbleEnergy < MaxUseAbleEnergy)
        {
            int SumAmount = ExchangeAmount + bonusAmount;
            int temp = (int)(SumAmount * bonusRate);
            Cooldown -= Time.deltaTime;
            if (Cooldown <= 0f)
            {
                if (temp <= ExchangeAbleEnergy)
                {
                    ExchangeAbleEnergy = ExchangeAbleEnergy - temp;
                    UseAbleEnergy = UseAbleEnergy + temp;
                } else
                {
                    temp = ExchangeAbleEnergy;
                    ExchangeAbleEnergy = ExchangeAbleEnergy - temp;
                    UseAbleEnergy = UseAbleEnergy + temp;
                }
                Cooldown = 1f;
            }
        }
    }
}
