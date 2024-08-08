using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Character : MonoBehaviour
{
    [SerializeField] private float SumMaxhealth;
    public float maxhealth = 100f;
    public float health;
    public float attackSpeed = 1f;
    public float attackDamage = 50;
    public float armor = 5;
    public float regenAmount = 0.1f;

    private float SumHealth;
    private float SumAttackSpeed;
    private float SumAttackDamage;
    private float SumArmor;
    private float SumRegenAmount;

    

    private float attackCooldown = 0f;
    private float regenCooldown = 0f;

    private List<Enemy> enemies;
    private Enemy currentTarget;
    public GameObject damageText;
    public TMP_Text popupText;
    public float increase;
    public Canvas canvas;
    bool isFirstHit = true;
    public StaticInventoryDisplay GearInventory;

    void Start()
    {
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        health = maxhealth;
        SelectNewTarget();
    }

    void Update()
    {
        UpdatePlayerStast();
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = 1f / SumAttackSpeed;
        }
        if (health < SumMaxhealth)
        {
            regenCooldown -= Time.deltaTime;
            if (regenCooldown <= 0f)
            {
                RegenHealth();
                regenCooldown = 1f;
            }
        }
        if (health > SumMaxhealth)
        {
            health = SumMaxhealth;
        }
    }



    public void UpdatePlayerStast()
    {
        SumMaxhealth = maxhealth + GearInventory.GearMaxhealth;
        SumAttackDamage = attackDamage + GearInventory.GearAttackDamage;
        SumAttackSpeed = attackSpeed + GearInventory.GearAttackSpeed;
        SumArmor = armor + GearInventory.GearArmor;
        SumRegenAmount = regenAmount + GearInventory.GearRegenAmount;
    }

    public void RegenHealth()
    {
        health += SumRegenAmount;    
    }

    public void EquipGear(InventoryItemData gearToEquip)
    {
        health += gearToEquip.bonusHealth;
        attackDamage += gearToEquip.bonusDamage;
        armor += gearToEquip.bonusArmor;
        attackSpeed += gearToEquip.bonusAttackSpeed;
        regenAmount += gearToEquip.bonusRegen;
    }

    public void UnEquipGear(InventoryItemData gearToEquip)
    {
        health -= gearToEquip.bonusHealth;
        attackDamage -= gearToEquip.bonusDamage;
        armor -= gearToEquip.bonusArmor;
        attackSpeed -= gearToEquip.bonusAttackSpeed;
        regenAmount -= gearToEquip.bonusRegen;
    }

    void Attack()
    {
        enemies.RemoveAll(enemy => enemy == null);

        if (currentTarget == null)
        {
            SelectNewTarget();
        }

        if (currentTarget != null)
        {
            currentTarget.TakeDamage(attackDamage);
        }
    }

    void SelectNewTarget()
    {
        enemies.RemoveAll(enemy => enemy == null);
        if (enemies.Count > 0)
        {
            int randomIndex = Random.Range(0, enemies.Count);
            currentTarget = enemies[randomIndex];
        }
        else
        {
            currentTarget = null;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isFirstHit)
        {
            float amount = damage - armor;
            if (amount < 1)
            {
                amount = 1;
            }
            health -= amount;
            RectTransform textTramform = Instantiate(damageText).GetComponent<RectTransform>();
            textTramform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + new Vector3(0, increase, 0);

            popupText.text = amount.ToString();
            textTramform.SetParent(canvas.transform);

            if (health <= 0f)
            {
                Debug.LogError("Character has died.");
            }
        } else
        {
            isFirstHit = false;
            return;
        }
        
    }

    public void EnemyDestroyed(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemy == currentTarget)
        {
            SelectNewTarget();
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        if (currentTarget == null)
        {
            SelectNewTarget();
        }
    }
}
