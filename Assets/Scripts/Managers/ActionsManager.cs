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
    private bool isCamera1Active = false;


    public GameObject TimePanel;

    private void Start()
    {
        TimePanel.SetActive(false);
        MainCamera.gameObject.SetActive(true);
        Camera1.gameObject.SetActive(false);
    }

    void Update()
    {
        Pickup();
    }

    public void Pickup()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            InventoryHolder.AddToInventory(itemData1, 1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            InventoryHolder.AddToInventory(itemData2, 1);
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

    public void SpeedUpTime(float multiplier)
    {
        Time.timeScale = multiplier;
    }

    public void ResetTime()
    {
        Time.timeScale = 1f;
    }

    public void SwitchCamera()
    {
        isCamera1Active = !isCamera1Active;

        MainCamera.gameObject.SetActive(isCamera1Active);
        Camera1.gameObject.SetActive(!isCamera1Active);
    }
} 
