using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public EquipableTag slotTag;
    public InventoryItemData equippedItem;
    public Image icon;

    public void EquipItem(InventoryItemData item)
    {
        if (item.EquipableTag == slotTag)
        {
            equippedItem = item;
            icon.sprite = item.icon;
            icon.enabled = true;
        }
    }

    public void UnequipItem()
    {
        equippedItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }
}
