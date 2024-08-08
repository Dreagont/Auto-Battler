using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlotUi[] slots;

    public float GearMaxhealth;
    public float GearAttackSpeed;
    public float GearAttackDamage;
    public float GearArmor;
    public float GearRegenAmount;

    private int oldfilledSlots = 0;

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
            inventorySystem = inventoryHolder.PrimaryInventorySystem;
            InventorySystem.OnInventorySlotsChanged += UpdateSlotStatic;
            AssignSlot(inventorySystem);
        }
        else
        {
            Debug.LogWarning("no inventoryHolder assigned to StaticInventoryDisplay");
        }
    }

    private void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            WhatIsThat();
        }

        int filledSlots = CountFilledSlots();

        if (filledSlots != oldfilledSlots)
        {
            oldfilledSlots = filledSlots;
            GetGearStats(inventorySystem);
        }
    }

    public void WhatIsThat()
    {
        Debug.Log(inventorySystem.InventorySlots[0].ItemData.displayName.ToString());
    }

    public void GetGearStats(InventorySystem inventoryToDisplay)
    {
        GearMaxhealth = 0;
        GearArmor = 0;
        GearAttackDamage = 0;
        GearAttackSpeed = 0;
        GearRegenAmount = 0;
        for (int i = 0; i < inventoryToDisplay.InventorySize; i++)
        {
            if (inventoryToDisplay.InventorySlots[i].ItemData != null)
            {
                GearMaxhealth += inventoryToDisplay.InventorySlots[i].ItemData.bonusHealth;
                GearArmor += inventoryToDisplay.InventorySlots[i].ItemData.bonusArmor;
                GearAttackDamage += inventoryToDisplay.InventorySlots[i].ItemData.bonusDamage;
                GearAttackSpeed += inventoryToDisplay.InventorySlots[i].ItemData.bonusAttackSpeed;
                GearRegenAmount += inventoryToDisplay.InventorySlots[i].ItemData.bonusRegen;
            }
        }
    }

    public int CountFilledSlots()
    {
        int filledSlotsCount = 0;

        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            if (inventorySystem.InventorySlots[i].ItemData != null)
            {
                filledSlotsCount++;
            }
        }

        return filledSlotsCount;
    }
}
