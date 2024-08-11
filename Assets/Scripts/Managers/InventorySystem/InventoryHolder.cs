using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem primaryInventorySystem;

    public InventorySystem PrimaryInventorySystem => primaryInventorySystem;
    public static UnityAction<InventorySystem> onDynamicInventoryDisplayRequested;

    protected virtual void Awake()
    {
        primaryInventorySystem = new InventorySystem(inventorySize);
    }
}
[System.Serializable]
public struct InventorySaveData
{
    public InventorySystem InventorySystem;

    public InventorySaveData(InventorySystem inventorySystem)
    {
        this.InventorySystem = inventorySystem;

    }

}