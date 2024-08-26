using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    public PlayerInventoryHolder InventoryHolder;
    public InventoryItemData itemData1;
    public InventoryItemData itemData2;

    public GlobalResourceManager globalResourceManager;

    public Camera MainCamera;
    public Camera Camera1;
    public GameObject FakeScreen;
    //private bool isCamera1Active = true;


    public GameObject TimePanel;
    public GameObject BarsPanel;

    private void Start()
    {
        TimePanel.SetActive(false);
        MainCamera.gameObject.SetActive(true);
        Camera1.gameObject.SetActive(false);
        //FakeScreen.SetActive(true);
    }

    void Update()
    {
        Pickup();
    }

    public void Pickup()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            InventoryHolder.AddToInventory(itemData1, 10);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            InventoryHolder.AddToInventory(itemData2, 10);
        }
    }
    public void PickupItem(InventoryItemData itemData)
    {
        InventoryHolder.AddToInventory(itemData, 1);
    }

    public void GainGold(int amount)
    {
        globalResourceManager.Gold += amount;
    }

    public void ToggleTimePanel()
    {
        if (TimePanel.gameObject.activeInHierarchy)
        {
            TimePanel.SetActive(false) ;
        } else
        {
            TimePanel.SetActive(true);
        }
    }

    public void ToggleBars()
    {
        BarsPanel.gameObject.SetActive(!BarsPanel.gameObject.activeInHierarchy);
    }

    public void SpeedUpTime(float multiplier)
    {
        Time.timeScale = multiplier;
    }

    public void ResetTime()
    {
        Time.timeScale = 1f;
    }

    public void SwitchCamera(int index)
    {
        if (index == 0)
        {
            MainCamera.gameObject.SetActive(false);
            Camera1.gameObject.SetActive(true);
        } else
        {
            MainCamera.gameObject.SetActive(true);
            Camera1.gameObject.SetActive(false);
        }

    }

    public void SelectStartMode(int index)
    {
        if (index == 0)
        {
            SaveGameManager.TryLoadData();
        }

        FakeScreen.gameObject.SetActive(false);
    }
} 
