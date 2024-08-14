using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUi : MonoBehaviour, IPointerClickHandler
{
    public Image itemSprite;
    public TextMeshProUGUI itemCount;
    public Image holder;
    public EquipableTag equipableTag = EquipableTag.None;
    [SerializeField] private InventorySlots assignedInventorySlot;

    public InventorySlots AssignedInventorySlot => assignedInventorySlot;

    private Button button;

    public InventoryDisplay ParentDisplay { get; private set; }

    private void Awake()
    {
        ClearSlot();

        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick); // This handles left-clicks

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
            holder.color = Color.clear;
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
        assignedInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
        if (equipableTag != EquipableTag.None)
        {
            holder.color = Color.white;
        }
    }

    public void ClearSlot(InventorySlots slots)
    {
        slots.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotLeftClicked(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnUISlotClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnUISlotRightClick();
        }
    }

    public void OnUISlotRightClick()
    {
        ParentDisplay?.SlotRightClicked(this);
        Debug.Log("Right-clicked on slot: " + name);
    }
}
