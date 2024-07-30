using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlots
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int stackSize;

    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;

    public InventorySlots(InventoryItemData source, int amount)
    {
        this.itemData = source;
        this.stackSize = amount;
    }

    public InventorySlots()
    {
        ClearSlot();
    }

    public void UpdateInventorySlot(InventoryItemData item, int amount)
    {
        itemData = item;
        stackSize = amount;
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }

    public void ClearSlot()
    {
        itemData = null;
        stackSize = 0;
    }

    public bool RoomLeftInStack(int amountToAdd)
    {
        return stackSize + amountToAdd <= itemData.MaxStackSize;
    }
}
