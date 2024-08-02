using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUi : MonoBehaviour
{
    public Image itemSprite;
    public TextMeshProUGUI itemCount;
    [SerializeField] private InventorySlots assignedInventorySlot;

    public InventorySlots AssignedInventorySlot => assignedInventorySlot;

    private Button button;

    public InventoryDisplay ParentDisplay { get; private set; }

    private void Awake()
    {
        ClearSlot();

        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void Init(InventorySlots slot)
    {
        assignedInventorySlot = slot;
        UpdateInventorySlot(slot);
    }

    public void UpdateInventorySlot(InventorySlots slot)
    {
        if (slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.icon;
            itemSprite.color = Color.white;

            itemCount.text = slot.StackSize > 1 ? slot.StackSize.ToString() : "";
        }
        else
        {
            ClearSlot();
        }
    }

    public void UpdateInventorySlot()
    {
        if (assignedInventorySlot != null)
        {
            UpdateInventorySlot(assignedInventorySlot);
        }
    }

    public void ClearSlot()
    {
        assignedInventorySlot.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }
}
