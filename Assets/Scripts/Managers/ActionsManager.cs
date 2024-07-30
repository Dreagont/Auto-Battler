using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    public InventoryHolder InventoryHolder;
    public InventoryItemData itemData;

    void Update()
    {
        Pickup();
    }

    public void Pickup()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            InventoryHolder.InventorySystem.AddToInventory(itemData, 1);
            Debug.Log("Picked up item: " + itemData.name);
        }
    }
}
