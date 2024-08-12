using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    public PlayerInventoryHolder InventoryHolder;
    public InventoryItemData itemData1;
    public InventoryItemData itemData2;

    public GlobalResourceManager globalResourceManager;

    void Update()
    {
        Pickup();
    }

    public void Pickup()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            InventoryHolder.AddToInventory(itemData1, 1);
            Debug.Log("Picked up item: " + itemData1.name);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            InventoryHolder.AddToInventory(itemData2, 1);
            Debug.Log("Picked up item: " + itemData2.name);
        }
    }
    public void PickupItem(InventoryItemData itemData)
    {
        InventoryHolder.AddToInventory(itemData, 1);
        Debug.Log("Picked up item: " + itemData.name);
    }

    public void GainGold(int amount)
    {
        globalResourceManager.Gold += amount;
    }
} 
