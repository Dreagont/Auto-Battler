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

            if (slot.StackSize > 1)
            {
                itemCount.text = slot.StackSize.ToString();
            }
            else
            {
                itemCount.text = "";
            }
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
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }
}
