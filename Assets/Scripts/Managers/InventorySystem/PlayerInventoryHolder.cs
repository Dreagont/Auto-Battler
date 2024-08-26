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

    public AutomationHouse farmerHouse;
    public AutomationHouse traderHouse;
    public CapacityHouse lummerHouse;
    public CapacityHouse minerHouse;
    public BlacksmithHouse blacksmithHouse;
    public BuilderHouse builderHouse;
    private Fighter character;

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
        character = GetComponent<Fighter>();
        SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
        SaveGameManager.data.playerEquipment = new InventorySaveData(secondaryInventorySystem);
        SaveGameManager.data.playerAutoSell = new InventorySaveData(sellInventorySystem);
        SaveGameManager.data.playerAutoUse = new InventorySaveData(useInventorySystem);

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


    public void LoadAll(SaveData data)
    {
        LoadInventory(data);
        LoadPlayerStats(data);
        LoadCharData(data);
        LoadHouseData(data);

        globalResourceManager.LoadResourceData(data);
        enemySpawner.LoadSpawnerData(data);
    }

    public void LoadCharData(SaveData data)
    {
        Miner miner = FindAnyObjectByType<Miner>();
        Builder builder = FindAnyObjectByType<Builder>();
        Farmer farmer = FindAnyObjectByType<Farmer>();
        Trader trader = FindAnyObjectByType<Trader>();
        Blacksmith blacksmith = FindAnyObjectByType<Blacksmith>();
        Lumberjack lumberjack = FindAnyObjectByType<Lumberjack>();

        miner.LoadSaveData();
        builder.LoadSaveData();
        farmer.LoadSaveData();
        trader.LoadSaveData();
        blacksmith.LoadSaveData();
        lumberjack.LoadSaveData();
    }

    public void LoadHouseData(SaveData data)
    {
        minerHouse.LoadSaveData();
        builderHouse.LoadSaveData();
        farmerHouse.LoadSaveData();
        traderHouse.LoadSaveData();
        blacksmithHouse.LoadSaveData();
        lummerHouse.LoadSaveData();
    }

    private void LoadInventory(SaveData data)
    {
        if (data != null && data.playerInventory.InventorySystem != null && data.playerEquipment.InventorySystem != null)
        {
            //ClearInventory(primaryInventorySystem);
            //ClearInventory(secondaryInventorySystem);
            //ClearInventory(sellInventorySystem);
            //ClearInventory(useInventorySystem);

            this.primaryInventorySystem = data.playerInventory.InventorySystem;
            this.secondaryInventorySystem = data.playerEquipment.InventorySystem;
            this.sellInventorySystem = data.playerAutoSell.InventorySystem;
            this.useInventorySystem = data.playerAutoUse.InventorySystem;
            character.inventorySystem = this.primaryInventorySystem;
        }
    }

    private void LoadPlayerStats(SaveData data)
    {
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
        SaveGameManager.data.playerBaseMaxHealth = GetComponent<Fighter>().baseMaxHealth;
        SaveGameManager.data.playerHealth = GetComponent<Fighter>().health;
        SaveGameManager.data.playerBaseAttackSpeed = GetComponent<Fighter>().baseAttackSpeed;
        SaveGameManager.data.playerBaseAttackDamage = GetComponent<Fighter>().baseAttackDamage;
        SaveGameManager.data.playerBaseArmor = GetComponent<Fighter>().baseArmor;
        SaveGameManager.data.playerBaseRegenAmount = GetComponent<Fighter>().baseRegenAmount;

        SaveGameManager.data.playerLevel = GetComponent<Fighter>().level;
        SaveGameManager.data.playerExperience = GetComponent<Fighter>().experience;
        SaveGameManager.data.playerBaseExperienceToNextLevel = GetComponent<Fighter>().baseExperienceToNextLevel;
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
