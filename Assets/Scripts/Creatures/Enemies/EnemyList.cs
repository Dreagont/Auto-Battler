using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDrop
{
    public InventoryItemData item;
    public int minQuantity = 1;
    public int maxQuantity = 1;
    public float itemDropChance = 0.5f;
}

[CreateAssetMenu(menuName = "Enemy System/Enemy Type")]
public class EnemyTypeData : ScriptableObject
{
    public string enemyTypeName;
    public float maxHealth;
    public float attackSpeed = 1;
    public float armor;
    public float damage;
    public int level = 1;
    public int gold = 10;
    public List<ItemDrop> dropItems;
}