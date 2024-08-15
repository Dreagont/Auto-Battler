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

public enum EnemyTraits
{
    Normal,
    Elite,
    Boss,
    ChapterBoss
}

[CreateAssetMenu(menuName = "Enemy System/Enemy Type")]
public class EnemyTypeData : ScriptableObject
{
    public string enemyTypeName;
    public float baseMaxHealth = 50f;
    public float baseAttackSpeed = 0.8f;
    public float baseArmor = 2f;
    public float baseDamage = 5f;
    public int baseGold = 10;
    public float baseExperienceValue = 20f;
    public int level = 1;
    public EnemyTraits enemyTraits = EnemyTraits.Normal;
    public List<ItemDrop> dropItems;

    private static readonly Dictionary<EnemyTraits, float> traitMultipliers = new Dictionary<EnemyTraits, float>
    {
        { EnemyTraits.Normal, 1f },
        { EnemyTraits.Elite, 2f },
        { EnemyTraits.Boss, 5f },
        { EnemyTraits.ChapterBoss, 10f }
    };

    public float GetMaxHealth(int level)
    {
        return baseMaxHealth * (1f + (level - 1) * 0.15f) * traitMultipliers[enemyTraits];
    }

    public float GetAttackSpeed(int level)
    {
        return baseAttackSpeed * (1f + (level - 1) * 0.05f);
    }

    public float GetArmor(int level)
    {
        return baseArmor * (1f + (level - 1) * 0.1f) * traitMultipliers[enemyTraits];
    }

    public float GetDamage(int level)
    {
        return baseDamage * (1f + (level - 1) * 0.12f) * traitMultipliers[enemyTraits];
    }

    public int GetGold(int level)
    {
        return Mathf.RoundToInt(baseGold * (1f + (level - 1) * 0.2f) * traitMultipliers[enemyTraits]);
    }

    public float GetExperienceValue(int level)
    {
        return baseExperienceValue * (1f + (level - 1) * 0.25f) * traitMultipliers[enemyTraits];
    }
}

[CreateAssetMenu(menuName = "Enemy System/Goblin")]
public class GoblinData : EnemyTypeData
{
    public GoblinData()
    {
        enemyTypeName = "Goblin";
        baseMaxHealth = 30f;
        baseAttackSpeed = 1.2f;
        baseArmor = 1f;
        baseDamage = 5f;
        baseGold = 5;
        baseExperienceValue = 10f;
        level = 1;
        enemyTraits = EnemyTraits.Normal;
    }
}

[CreateAssetMenu(menuName = "Enemy System/Orc")]
public class OrcData : EnemyTypeData
{
    public OrcData()
    {
        enemyTypeName = "Orc";
        baseMaxHealth = 80f;
        baseAttackSpeed = 0.8f;
        baseArmor = 3f;
        baseDamage = 12f;
        baseGold = 15;
        baseExperienceValue = 25f;
        level = 1;
        enemyTraits = EnemyTraits.Normal;
    }
}

[CreateAssetMenu(menuName = "Enemy System/Mage")]
public class MageData : EnemyTypeData
{
    public MageData()
    {
        enemyTypeName = "Mage";
        baseMaxHealth = 40f;
        baseAttackSpeed = 0.7f;
        baseArmor = 1f;
        baseDamage = 15f;
        baseGold = 20;
        baseExperienceValue = 30f;
        level = 1;
        enemyTraits = EnemyTraits.Normal;
    }
}

[CreateAssetMenu(menuName = "Enemy System/Troll")]
public class TrollData : EnemyTypeData
{
    public TrollData()
    {
        enemyTypeName = "Troll";
        baseMaxHealth = 150f;
        baseAttackSpeed = 0.5f;
        baseArmor = 5f;
        baseDamage = 20f;
        baseGold = 25;
        baseExperienceValue = 40f;
        level = 1;
        enemyTraits = EnemyTraits.Normal;
    }
}

[CreateAssetMenu(menuName = "Enemy System/Wolf")]
public class WolfData : EnemyTypeData
{
    public WolfData()
    {
        enemyTypeName = "Wolf";
        baseMaxHealth = 35f;
        baseAttackSpeed = 1.5f;
        baseArmor = 1f;
        baseDamage = 7f;
        baseGold = 7;
        baseExperienceValue = 12f;
        level = 1;
        enemyTraits = EnemyTraits.Normal;
    }
}

