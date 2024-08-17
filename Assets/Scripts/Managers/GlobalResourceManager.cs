using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalResourceManager : MonoBehaviour
{
    public int Gold = 0;
    public int ExchangeAbleEnergy = 1000;
    public int UseAbleEnergy;
    public int Woods;
    public int Ores;

    public int MaxExchangeAbleEnergy = 2000;
    public int MaxUseAbleEnergy = 100;
    public int MaxWoods = 100;
    public int MaxOres = 100;

    public int ExchangeAmount = 10;

    public TMP_Text GoldText;
    public TMP_Text ExchangeAbleEnergyText;
    public TMP_Text UseAbleEnergyText;
    public TMP_Text MaxExchangeAbleEnergyText;
    public TMP_Text MaxUseAbleEnergyText;
    public TMP_Text WoodsText;
    public TMP_Text OresText;

    private float elapsedTime = 0f;
    public TMP_Text timerText;

    private float Cooldown = 0f;

    public BarManager UseBar;
    public BarManager ExchangBar;

    public Lumberjack Lumberjack;
    public Miner Miner;
    private void Start()
    {
        SaveResourceData();
    }

    private void Update()
    {
        UpdateBars();
        
        UpdateText();
        
        SaveResourceData();

        Timming();

        Limmiter(ref UseAbleEnergy, MaxUseAbleEnergy);
        Limmiter(ref ExchangeAbleEnergy, MaxExchangeAbleEnergy);
        Limmiter(ref Woods, MaxWoods);
        Limmiter(ref Ores, MaxOres);

        GainResource();
    }

    public void SaveResourceData()
    {
        SaveGameManager.data.gold = Gold;
        SaveGameManager.data.exchangeableEnergy = ExchangeAbleEnergy;
        SaveGameManager.data.usableEnergy = UseAbleEnergy;
        SaveGameManager.data.maxExchangeableEnergy = MaxExchangeAbleEnergy;
        SaveGameManager.data.maxUsableEnergy = MaxUseAbleEnergy;
        SaveGameManager.data.elapsedTime = elapsedTime;
    }

    public void LoadResourceData(SaveData data)
    {
        Gold = data.gold;
        ExchangeAbleEnergy = data.exchangeableEnergy;
        UseAbleEnergy = data.usableEnergy;
        MaxExchangeAbleEnergy = data.maxExchangeableEnergy;
        MaxUseAbleEnergy = data.maxUsableEnergy;
        elapsedTime = data.elapsedTime;

        UpdateUI();
    }

    private void UpdateUI()
    {
        GoldText.text = ReuseMethod.FormatNumber(Gold);
        ExchangeAbleEnergyText.text = ReuseMethod.FormatNumber(ExchangeAbleEnergy);
        UseAbleEnergyText.text = ReuseMethod.FormatNumber(UseAbleEnergy);
        MaxExchangeAbleEnergyText.text = ReuseMethod.FormatNumber(MaxExchangeAbleEnergy);
        MaxUseAbleEnergyText.text = ReuseMethod.FormatNumber(MaxUseAbleEnergy);
        UseBar.UpdateBar(UseAbleEnergy, MaxUseAbleEnergy);
        ExchangBar.UpdateBar(ExchangeAbleEnergy, MaxExchangeAbleEnergy);
    }

    public void UpdateBars()
    {
        UseBar.UpdateBar(UseAbleEnergy, MaxUseAbleEnergy);
        ExchangBar.UpdateBar(ExchangeAbleEnergy, MaxExchangeAbleEnergy);
    }

    public void Timming()
    {
        elapsedTime += Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = ReuseMethod.FormatTime(elapsedTime);
        }
    }

    public void UpdateText()
    {
        GoldText.text = ReuseMethod.FormatNumber(Gold);
        ExchangeAbleEnergyText.text = ReuseMethod.FormatNumber(ExchangeAbleEnergy);
        UseAbleEnergyText.text = ReuseMethod.FormatNumber(UseAbleEnergy);
        MaxExchangeAbleEnergyText.text = ReuseMethod.FormatNumber(MaxExchangeAbleEnergy);
        MaxUseAbleEnergyText.text = ReuseMethod.FormatNumber(MaxUseAbleEnergy);
        WoodsText.text = ReuseMethod.FormatNumber(Woods);
        OresText.text = ReuseMethod.FormatNumber(Ores);
    }

    public void Limmiter(ref int currentValue, int maxValue)
    {
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
        else if (currentValue < 0)
        {
            currentValue = 0;
        }
    }

    public void GainResource()
    {
        Lumberjack.ChopWood();
        Miner.MineOre();
        ExchangeEnergy(0, 1);
    }


    public void ExchangeEnergy(int bonusAmount, float bonusRate)
    {
        if (ExchangeAbleEnergy <= 0)
        {
            ExchangeAbleEnergy = 0;
        } else
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
                    }
                    else
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

}
