using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        ParentDisplay?.SlotClicked(this);
    }

    private Coroutine showTooltipCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (assignedInventorySlot.ItemData != null)
        {
            showTooltipCoroutine = StartCoroutine(ShowTooltipDelayed());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showTooltipCoroutine != null)
        {
            StopCoroutine(showTooltipCoroutine);
        }
        TooltipManager.instance.HideTooltip();
    }

    private IEnumerator ShowTooltipDelayed()
    {
        yield return new WaitForSeconds(0.5f); // Adjust this delay as needed
        TooltipManager.instance.SetAndShowToolTip(assignedInventorySlot.ItemData.icon,assignedInventorySlot.ItemData.displayName, assignedInventorySlot.ItemData.description);
    }
}
