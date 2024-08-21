using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UseInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlotUi[] slots;

    public Farmer Farmer;

    private Coroutine autoUseCoroutine;
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
            inventorySystem = inventoryHolder.UseInventorySystem;
            InventorySystem.OnInventorySlotsChanged += UpdateSlotStatic;
            AssignSlot(inventorySystem);
        }
        else
        {
            Debug.LogWarning("no inventoryHolder assigned to StaticInventoryDisplay");
        }
        StartAutoUse();
    }


    void Update()
    {

    }

    private IEnumerator AutoUseCoroutine()
    {
        while (true)
        {
            AutoUse();
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

    public void AutoUse()
    {
        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            if (inventorySystem.InventorySlots[i].ItemData != null)
            {
                if (inventorySystem.InventorySlots[i].StackSize >= Farmer.UseAmount)
                {
                    Farmer.AutoUseItem(inventorySystem.InventorySlots[i].ItemData, Farmer.UseAmount);
                    inventorySystem.InventorySlots[i].RemoveFromStack(Farmer.UseAmount);
                }
                else
                {
                    Farmer.AutoUseItem(inventorySystem.InventorySlots[i].ItemData, inventorySystem.InventorySlots[i].StackSize);
                    inventorySystem.InventorySlots[i].RemoveFromStack(inventorySystem.InventorySlots[i].StackSize);
                }
                UpdateSlotStatic(inventorySystem.InventorySlots[i]);

            }
        }
    }
}
