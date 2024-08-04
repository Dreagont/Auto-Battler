using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] private int secondayInventorySize;
    [SerializeField] protected InventorySystem secondaryInventorySystem;
    public ChestInventory chestInventory;

    public InventorySystem SecondaryInventorySystem => secondaryInventorySystem;

    protected override void Awake()
    {
        base.Awake(); 
        secondaryInventorySystem = new InventorySystem(secondayInventorySize);
    }
    
    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            onDynamicInventoryDisplayRequested?.Invoke(SecondaryInventorySystem);
        }
    }

    public bool AddToInventory(InventoryItemData inventoryItemData, int amount)
    {
        if (chestInventory != null && chestInventory.chestInventory.AddToInventory(inventoryItemData, amount))
        {
            chestInventory.inventoryPanel.RefreshDynamicInventory(chestInventory.chestInventory);
            return true;
        }


        return false;
    }
}
