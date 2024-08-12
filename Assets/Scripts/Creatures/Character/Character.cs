using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Character : MonoBehaviour
{
    [SerializeField] private float SumMaxhealth;
    public float maxHealth = 100f;
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

    public int level = 1;
    public float experience = 0f;
    public float experienceToNextLevel = 100f;
    public float experienceMultiplier = 1.5f;

    private List<Enemy> enemies;
    private Enemy currentTarget;
    public GameObject damageText;
    public TMP_Text popupText;
    public float increase;
    public Canvas canvas;
    bool isFirstHit = true;
    public StaticInventoryDisplay GearInventory;
    public EnemyHealthBar healthBar;


    void Start()
    {
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        health = maxHealth;
        healthBar.UpdateBar(health, maxHealth);
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

        if (health < 0)
        {
            health = 0;
        }
    }

    public void UpdatePlayerStast()
    {
        SumMaxhealth = maxHealth + GearInventory.GearMaxhealth;
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
            currentTarget.TakeDamage(SumAttackDamage);
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
            healthBar.UpdateBar(health, maxHealth);

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

        GainExperience(enemy.enemyTypeData.level * 10);
    }
    void GainExperience(float amount)
    {
        experience += amount;

        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        experience -= experienceToNextLevel;
        experienceToNextLevel *= experienceMultiplier;

        maxHealth *= 1.05f;
        attackDamage *= 1.05f;
        armor *= 1.05f;
        regenAmount *= 1.05f;
        attackSpeed *= 1.05f;

        health = maxHealth;

        Debug.Log("Leveled Up! New Level: " + level);
    }
    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        if (currentTarget == null)
        {
            SelectNewTarget();
        }
    }

    internal bool IsHealthFull() => health == SumMaxhealth;

    internal bool IsDead() => health <= 0;
}
