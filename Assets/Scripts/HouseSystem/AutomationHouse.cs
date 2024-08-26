using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomationHouse : CapacityHouse
{
    public GameObject[] slots;

    void Start()
    {
        InitSaveData();
        UpdateSlots();
    }

    void Update()
    {
        InitSaveData();
    }

    public void UnLockSlot()
    {
        if (HouseLevel % 5 == 0)
        {
            int slotIndex = (HouseLevel / 5) - 1; 

            if (slotIndex >= 0 && slotIndex < slots.Length)
            {
                slots[slotIndex].gameObject.SetActive(true); 
            }
        }
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (HouseLevel >= (i + 1) * 5)
            {
                slots[i].SetActive(true); 
            }
            else
            {
                slots[i].SetActive(false); 
            }
        }
    }

    public void OnLevelUp()
    {
        HouseLevel++;
        UnLockSlot();
    }
}
