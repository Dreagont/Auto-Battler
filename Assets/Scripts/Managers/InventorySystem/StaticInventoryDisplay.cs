using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
        int filledSlots = CountFilledSlots();

        if (filledSlots != oldfilledSlots)
        {
            oldfilledSlots = filledSlots;
            GetGearStats(inventorySystem);
        }
        
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
            var slot = inventoryToDisplay.InventorySlots[i];
            if (slot.ItemData != null)
            {
                int quantity = slot.StackSize;
                float stackMultiplier = 1 + (0.05f * (quantity - 1));

                GearMaxhealth += slot.ItemData.bonusHealth * stackMultiplier;
                GearArmor += slot.ItemData.bonusArmor * stackMultiplier;
                GearAttackDamage += slot.ItemData.bonusDamage * stackMultiplier;
                GearAttackSpeed += slot.ItemData.bonusAttackSpeed * stackMultiplier;
                GearRegenAmount += slot.ItemData.bonusRegen * stackMultiplier;
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


