using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    public static SaveData data;

    private void Awake()
    {
        if (data == null)
        {
            data = new SaveData();
        }

        SaveLoad.OnLoadGame += LoadData;
    }

    public static void SaveData()
    {
        if (data == null)
        {
            data = new SaveData();
        }

        SaveLoad.Save(data);
    }

    public void DeleteSave()
    {
        SaveLoad.DeleteSaveData();
    }

    public static void LoadData(SaveData _data)
    {
        data = _data;
    }

    public static void TryLoadData()
    {
        SaveLoad.Load();
    }
}
