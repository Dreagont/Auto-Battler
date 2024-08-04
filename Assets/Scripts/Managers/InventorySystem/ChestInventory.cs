using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ChestInventory : MonoBehaviour
{
    public GameObject InventoryUI;
    public DynamicInventoryDisplay inventoryPanel;
    public InventorySystem chestInventory;
    public Image chestImage;
    public Button addSlotButton;
    private void Awake()
    {
        chestInventory = new InventorySystem(48);

        InventoryUI.gameObject.SetActive(false);

        addSlotButton.onClick.AddListener(() => AddSlotsToInventory(0));

        if (chestImage != null)
        {
            chestImage.GetComponent<Button>().onClick.AddListener(OpenChest);
        }

        inventoryPanel.AssignSlot(chestInventory);
    }

    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            if (InventoryUI.gameObject.activeInHierarchy)
            {
                InventoryUI.gameObject.SetActive(false);
            } else
            {
                OpenChest();
            }
        }

        if (InventoryUI.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            InventoryUI.gameObject.SetActive(false);
        }
    }

    private void OpenChest()
    {
        if (InventoryUI.gameObject.activeInHierarchy)
        {
            InventoryUI.gameObject.SetActive(false);
        } else {
            InventoryUI.gameObject.SetActive(true);
            inventoryPanel.RefreshDynamicInventory(chestInventory);
        }
    }

    private void AddSlotsToInventory(int quantity)
    {
        inventoryPanel.AddSlots(quantity);
    }
}
