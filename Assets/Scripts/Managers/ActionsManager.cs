using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    public InventoryHolder InventoryHolder;
    public InventoryItemData itemData1;
    public InventoryItemData itemData2;


    void Update()
    {
        Pickup();
    }

    public void Pickup()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            InventoryHolder.InventorySystem.AddToInventory(itemData1, 1);
            Debug.Log("Picked up item: " + itemData1.name);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            InventoryHolder.InventorySystem.AddToInventory(itemData2, 1);
            Debug.Log("Picked up item: " + itemData2.name);
        }
    }
}
