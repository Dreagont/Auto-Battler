using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SaveData 
{
    public InventorySaveData playerInventory;
    public InventorySaveData playerEquipment;

    public SaveData()
    {
        playerInventory = new InventorySaveData();
        playerEquipment = new InventorySaveData();
    }
}
