using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SellInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlotUi[] slots;

    public Trader Trader;

    private Coroutine autoSellCoroutine;
    public override void AssignSlot(InventorySystem inventoryToDisplay)
    {
        slotDictionary = new Dictionary<InventorySlotUi, InventorySlots>();

        if (slots.Length != inventoryToDisplay.InventorySize)
        {
            Debug.LogWarning($"out of sync on {this.gameObject}");
        }

        for (int i = 0; i < inventoryToDisplay.InventorySize; i++)
        {
            slotDictionary.Add(slots[i], inventoryToDisplay.InventorySlots[i]);
            slots[i].Init(inventoryToDisplay.InventorySlots[i]);
        }
    }

    protected override void Start()
    {
        base.Start();
        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.SellInventorySystem;
            InventorySystem.OnInventorySlotsChanged += UpdateSlotStatic;
            AssignSlot(inventorySystem);
        }
        else
        {
            Debug.LogWarning("no inventoryHolder assigned to StaticInventoryDisplay");
        }
        StartAutoSell();
    }


    void Update()
    {
        
    }

    private IEnumerator AutoSellCoroutine()
    {
        while (true)
        {
            AutoSell();
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

    public void AutoSell()
    {
        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            if (inventorySystem.InventorySlots[i].ItemData != null)
            {
                if (inventorySystem.InventorySlots[i].StackSize >= Trader.GetSellAmount())
                {
                    Trader.SellItem(inventorySystem.InventorySlots[i].ItemData, Trader.GetSellAmount());
                    inventorySystem.InventorySlots[i].RemoveFromStack(Trader.GetSellAmount());
                }
                else
                {
                    Trader.SellItem(inventorySystem.InventorySlots[i].ItemData, inventorySystem.InventorySlots[i].StackSize);
                    inventorySystem.InventorySlots[i].RemoveFromStack(inventorySystem.InventorySlots[i].StackSize);
                }
                UpdateSlotStatic(inventorySystem.InventorySlots[i]);

            }
        }
    }
}
