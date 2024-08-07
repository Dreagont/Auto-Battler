using UnityEngine;

[CreateAssetMenu(menuName = "Enemy System/Enemy Type")]
public class EnemyTypeData : ScriptableObject
{
    public string enemyTypeName;
    public float maxHealth;
    public float attackSpeed = 1;
    public float armor;
    public float damage;
    public int level = 1;
}
