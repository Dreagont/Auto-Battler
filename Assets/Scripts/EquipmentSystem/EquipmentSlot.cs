using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public TMP_Text EquipmentName;
    public TMP_Text EquipmentGoldCost;
    public TMP_Text EquipmentOreCost;
    public InventorySlotUi SlotUi;
    public Blacksmith Blacksmith;
    public bool isUpgrade = false;
    public int SlotIndex = 0;

    private void Update()
    {
        UpdateSlotUI(SlotIndex);
    }

    public void UpdateSlotUI(int index)
    {
        EquipmentName.text = "LV" + Blacksmith.slotLevel[SlotIndex] + "." + SlotUi.equipableTag.ToString();
        if (isUpgrade)
        {
            EquipmentGoldCost.text = ReuseMethod.FormatNumber(Blacksmith.GetGoldCost(Blacksmith.slotLevel[SlotIndex]));
            EquipmentOreCost.text = ReuseMethod.FormatNumber(Blacksmith.GetOreCost(Blacksmith.slotLevel[SlotIndex]));
        }
    }

}
