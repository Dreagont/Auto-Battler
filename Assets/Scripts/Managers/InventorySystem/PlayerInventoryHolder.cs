using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.WSA;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] private int secondayInventorySize;
    [SerializeField] protected InventorySystem secondaryInventorySystem;

    public InventorySystem SecondaryInventorySystem => secondaryInventorySystem;
    public Image chestImage;

    public InventorySlotUi[] SlotUi;

    private GlobalResourceManager globalResourceManager;
    private EnemySpawner enemySpawner;
    
    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadGame += LoadAll;
        secondaryInventorySystem = new InventorySystem(secondayInventorySize);

        // Ensure data is initialized
        if (SaveGameManager.data == null)
        {
            SaveGameManager.data = new SaveData();
        }

        SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
        SaveGameManager.data.playerEquipment = new InventorySaveData(secondaryInventorySystem);

        InitPlayerStatsSave();
    }
    private void Start()
    {
        if (chestImage != null)
        {
            chestImage.GetComponent<Button>().onClick.AddListener(OpenChest);
        }

        globalResourceManager = FindObjectOfType<GlobalResourceManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            OpenChest();
        }
        InitPlayerStatsSave();
    }


    private void LoadAll(SaveData data)
    {
        LoadInventory(data);
        LoadPlayerStats(data);
        globalResourceManager.LoadResourceData(data);
        enemySpawner.LoadSpawnerData(data);
    }

    private void LoadInventory(SaveData data)
    {
        if (data != null && data.playerInventory.InventorySystem != null && data.playerEquipment.InventorySystem != null)
        {
            ClearInventory(primaryInventorySystem);
            ClearInventory(secondaryInventorySystem);

            this.primaryInventorySystem = data.playerInventory.InventorySystem;
            this.secondaryInventorySystem = data.playerEquipment.InventorySystem;
        }
    }

    private void LoadPlayerStats(SaveData data)
    {
        Character character = GetComponent<Character>();
        character.baseMaxHealth = data.playerBaseMaxHealth;
        character.health = data.playerHealth;
        character.baseAttackSpeed = data.playerBaseAttackSpeed;
        character.baseAttackDamage = data.playerBaseAttackDamage;
        character.baseArmor = data.playerBaseArmor;
        character.baseRegenAmount = data.playerBaseRegenAmount;

        character.level = data.playerLevel;
        character.experience = data.playerExperience;
        character.baseExperienceToNextLevel = data.playerBaseExperienceToNextLevel;

        character.healthBar.UpdateBar(character.health, character.baseMaxHealth);
        character.EXPBar.UpdateBar(character.experience, character.baseExperienceToNextLevel);
    }

    private void ClearInventory(InventorySystem inventory)
    {
        foreach (var slot in inventory.InventorySlots)
        {
            if (slot.ItemData != null)
            {
                slot.ClearSlot();
            }
        }

        foreach (var slot in SlotUi)
        {
            slot?.ClearSlot();
        }

    }

    private void InitPlayerStatsSave()
    { 
        SaveGameManager.data.playerBaseMaxHealth = GetComponent<Character>().baseMaxHealth;
        SaveGameManager.data.playerHealth = GetComponent<Character>().health;
        SaveGameManager.data.playerBaseAttackSpeed = GetComponent<Character>().baseAttackSpeed;
        SaveGameManager.data.playerBaseAttackDamage = GetComponent<Character>().baseAttackDamage;
        SaveGameManager.data.playerBaseArmor = GetComponent<Character>().baseArmor;
        SaveGameManager.data.playerBaseRegenAmount = GetComponent<Character>().baseRegenAmount;

        SaveGameManager.data.playerLevel = GetComponent<Character>().level;
        SaveGameManager.data.playerExperience = GetComponent<Character>().experience;
        SaveGameManager.data.playerBaseExperienceToNextLevel = GetComponent<Character>().baseExperienceToNextLevel;
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
