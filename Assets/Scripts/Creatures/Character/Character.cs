using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Character : MonoBehaviour
{
    [SerializeField] private float SumMaxhealth;
    public float baseMaxHealth = 100f;
    public float health;
    public float baseAttackSpeed = 1f;
    public float baseAttackDamage = 10f;
    public float baseArmor = 5;
    public float baseRegenAmount = 0.1f;

    private float SumHealth;
    private float SumAttackSpeed;
    private float SumAttackDamage;
    private float SumArmor;
    private float SumRegenAmount;

    private float attackCooldown = 0f;
    private float regenCooldown = 0f;

    public int level = 1;
    public float experience = 0f;
    public float baseExperienceToNextLevel = 100f;
    public float experienceMultiplier = 1.5f;

    private List<Enemy> enemies;
    private Enemy currentTarget;
    public GameObject damageText;
    public TMP_Text popupText;
    public float increase;
    public Canvas canvas;
    public StaticInventoryDisplay GearInventory;
    public BarManager healthBar;
    public BarManager EXPBar;
    public TextMeshProUGUI currentLevelText;

    public GlobalResourceManager GlobalResourceManager;
    private bool Died = false;

    void Start()
    {
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        health = baseMaxHealth;
        healthBar.UpdateBar(health, baseMaxHealth);
        EXPBar.UpdateBar(experience, baseExperienceToNextLevel);
        SelectNewTarget();
    }

    void Update()
    {
        UpdatePlayerStast();

        currentLevelText.text = "LV." + level.ToString();

        if (IsHealthFull())
        {
            Died = false ;
        }

        EXPBar.UpdateBar(experience, baseExperienceToNextLevel);

        int temp = GlobalResourceManager.UseAbleEnergy - 10;

        if (temp >= 0)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0f)
            {
                Attack();
                attackCooldown = 1f / SumAttackSpeed;
            }
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
        SumMaxhealth = baseMaxHealth + GearInventory.GearMaxhealth;
        SumAttackDamage = baseAttackDamage + GearInventory.GearAttackDamage;
        SumAttackSpeed = baseAttackSpeed + GearInventory.GearAttackSpeed;
        SumArmor = baseArmor + GearInventory.GearArmor;
        SumRegenAmount = baseRegenAmount + GearInventory.GearRegenAmount;
        healthBar.UpdateBar(health, SumMaxhealth);
    }

    public void RegenHealth()
    {
        if (Died)
        {
            health += SumRegenAmount * 10;
        } else
        {
            health += SumRegenAmount;
        }
        
        healthBar.UpdateBar(health, baseMaxHealth);
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
            GlobalResourceManager.UseAbleEnergy -= 10;
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
        float amount = damage - baseArmor;
        if (amount < 1)
        {
            amount = 1;
        }
        health -= amount;
        healthBar.UpdateBar(health, baseMaxHealth);

        GameObject damageTextInstance = Instantiate(damageText);
        RectTransform textTransform = damageTextInstance.GetComponent<RectTransform>();
        textTransform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, increase, 0);

        TMP_Text damageTextComponent = damageTextInstance.GetComponent<TMP_Text>();
        if (damageTextComponent != null)
        {
            damageTextComponent.text = ReuseMethod.FormatNumber(amount);
        }
        else
        {
            Debug.LogError("TMP_Text component not found on damage text prefab");
        }
        textTransform.SetParent(canvas.transform);

        if (health <= 0f)
        {
            Died = true;
        }
        
    }
    public void EnemyDestroyed(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemy == currentTarget)
        {
            SelectNewTarget();
        }

        GainExperience(enemy.enemyTypeData.GetExperienceValue(enemy.enemyTypeData.level));
    }
    void GainExperience(float amount)
    {
        experience += amount;

        if (experience >= baseExperienceToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        experience -= baseExperienceToNextLevel;
        baseExperienceToNextLevel *= experienceMultiplier;

        baseMaxHealth *= 1.05f;
        baseAttackDamage *= 1.05f;
        baseArmor *= 1.05f;
        baseRegenAmount *= 1.05f;
        baseAttackSpeed *= 1.05f;

        health = baseMaxHealth;

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
