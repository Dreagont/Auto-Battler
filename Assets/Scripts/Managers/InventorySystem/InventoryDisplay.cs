using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItemData mouseInventoryItem;

    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlotUi, InventorySlots> slotDictionary;

    public InventorySystem InventorySystem => inventorySystem;
    protected Dictionary<InventorySlotUi, InventorySlots> SlotDictionary => slotDictionary;

    public abstract void AssignSlot(InventorySystem inventoryToDisplay);

    protected virtual void UpdateSlot(InventorySlots updatedSlot)
    {
        foreach (var slot in slotDictionary)
        {
            if (slot.Value == updatedSlot)
            {
                slot.Key.UpdateInventorySlot(updatedSlot);
            }
        }
    }

    protected virtual void Start()
    {
    }

    public void SlotClicked(InventorySlotUi slot)
    {
        Debug.Log("Slot Clicked");

        if (slot.AssignedInventorySlot != null)
        {
            Debug.Log("slot ok");
        }
        
        if (mouseInventoryItem.AsssignedInventorySlot == null)
        {
            Debug.Log("mouse ok");
        }
        

        if (slot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AsssignedInventorySlot == null)
        {
            Debug.Log("Jump in");
            mouseInventoryItem.UpdateMouseSlot(slot.AssignedInventorySlot);
            slot.ClearSlot();
        }

        if (slot.AssignedInventorySlot.ItemData == null && mouseInventoryItem.AsssignedInventorySlot != null)
        {
            slot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AsssignedInventorySlot);
            slot.UpdateInventorySlot();

            mouseInventoryItem.ClearSlot();
        }
    }
}
