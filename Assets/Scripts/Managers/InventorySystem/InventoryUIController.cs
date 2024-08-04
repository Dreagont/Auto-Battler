using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    public DynamicInventoryDisplay inventoryPanel;

    private void Awake()
    {
        //inventoryPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryHolder.onDynamicInventoryDisplayRequested += DisplayInventory;
    }

    private void OnDisable()
    {
        InventoryHolder.onDynamicInventoryDisplayRequested -= DisplayInventory;

    }

    void Update()
    {

        //if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) inventoryPanel.gameObject.SetActive(false);

    }

    void DisplayInventory (InventorySystem inventoryToDisplay)
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.RefreshDynamicInventory(inventoryToDisplay);

    }
}
