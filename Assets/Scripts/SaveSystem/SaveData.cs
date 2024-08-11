using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SaveData 
{
    public SerializableDictionary<string, InventorySaveData> ChestDictionary;

    public SaveData()
    {
        ChestDictionary = new SerializableDictionary<string, InventorySaveData> ();
    }
}
