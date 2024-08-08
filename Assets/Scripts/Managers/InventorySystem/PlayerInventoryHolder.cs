using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] private int secondayInventorySize;
    [SerializeField] protected InventorySystem secondaryInventorySystem;

    public InventorySystem SecondaryInventorySystem => secondaryInventorySystem;
    public Image chestImage;

    protected override void Awake()
    {
        base.Awake(); 
        secondaryInventorySystem = new InventorySystem(secondayInventorySize);
    }

    private void Start()
    {
        if (chestImage != null)
        {
            chestImage.GetComponent<Button>().onClick.AddListener(OpenChest);
        }
    }

    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            OpenChest();
        }
        
    }

    public void OpenChest()
    {
        onDynamicInventoryDisplayRequested?.Invoke(SecondaryInventorySystem);
    }

    public bool AddToInventory(InventoryItemData inventoryItemData, int amount)
    {
        if (secondaryInventorySystem.AddToInventory(inventoryItemData, amount))
        {
            return true;
        }


        return false;
    }
}
