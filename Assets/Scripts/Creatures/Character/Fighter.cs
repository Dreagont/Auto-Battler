using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Fighter : MonoBehaviour
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

    public float GearMaxhealth;
    public float GearAttackSpeed;
    public float GearAttackDamage;
    public float GearArmor;
    public float GearRegenAmount;

    private int oldfilledSlots = 0;

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
    public PlayerInventoryHolder playerInventory;
    public InventorySystem inventorySystem;
    public BarManager healthBar;
    public BarManager EXPBar;
    public TextMeshProUGUI currentLevelText;

    public GlobalResourceManager GlobalResourceManager;
    public bool Died = false;
    
    public Blacksmith Blacksmith;

    void Start()
    {
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        health = baseMaxHealth;
        inventorySystem = playerInventory.PrimaryInventorySystem;
        healthBar.UpdateBar(health, baseMaxHealth);
        EXPBar.UpdateBar(experience, baseExperienceToNextLevel);
        SelectNewTarget();
    }

    void Update()
    {
        int filledSlots = CountFilledSlots(inventorySystem);

        if (filledSlots != oldfilledSlots)
        {
            oldfilledSlots = filledSlots;
            GetGearStats(inventorySystem);
        }

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
        SumMaxhealth = baseMaxHealth + GearMaxhealth;
        SumAttackDamage = baseAttackDamage + GearAttackDamage;
        SumAttackSpeed = baseAttackSpeed + GearAttackSpeed;
        SumArmor = baseArmor + GearArmor;
        SumRegenAmount = baseRegenAmount + GearRegenAmount;
        healthBar.UpdateBar(health, SumMaxhealth);
    }

    public void RegenHealth()
    {
        if (Died)
        {
            SpeedRegenHealth();
        } else
        {
            health += SumRegenAmount;
            healthBar.UpdateBar(health, baseMaxHealth);
        }

    }

    public void SpeedRegenHealth()
    {
        health += SumRegenAmount * 10;
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
        float amount = damage - SumArmor;
        if (amount < 1)
        {
            amount = 1;
        }
        health -= amount;
        healthBar.UpdateBar(health, baseMaxHealth);

        if (Camera.main != null)
        {
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
        }

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
        if (enemy != null)
        {
            enemies.Add(enemy);

        }
        if (currentTarget == null)
        {
            SelectNewTarget();
        }
    }

    internal bool IsHealthFull() => health == SumMaxhealth;

    internal bool IsDead() => health <= 0;

    public void GetGearStats(InventorySystem inventoryToDisplay)
    {
        // Reset gear stats
        GearMaxhealth = 0;
        GearArmor = 0;
        GearAttackDamage = 0;
        GearAttackSpeed = 0;
        GearRegenAmount = 0;

        Debug.LogWarning("getgear");

        for (int i = 0; i < inventoryToDisplay.InventorySize; i++)
        {
            var slot = inventoryToDisplay.InventorySlots[i];
            if (slot.ItemData != null)
            {
                int quantity = slot.StackSize;
                float stackMultiplier = 1 + (0.05f * (quantity - 1));
                int level = Blacksmith.slotLevel[i];

                // Calculate the stat bonus based on item level
                float levelMultiplier = 1 + 0.1f * (level - 1);

                GearMaxhealth += slot.ItemData.bonusHealth * stackMultiplier * levelMultiplier;
                GearArmor += slot.ItemData.bonusArmor * stackMultiplier * levelMultiplier;
                GearAttackDamage += slot.ItemData.bonusDamage * stackMultiplier * levelMultiplier;
                GearAttackSpeed += slot.ItemData.bonusAttackSpeed * stackMultiplier * levelMultiplier;
                GearRegenAmount += slot.ItemData.bonusRegen * stackMultiplier * levelMultiplier;
            } else
            {
                Debug.LogWarning("null here: "+ i);
            }
        }
    }


    public int CountFilledSlots(InventorySystem inventorySystem)
    {
        int filledSlotsCount = 0;

        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            if (inventorySystem.InventorySlots[i].ItemData != null)
            {
                filledSlotsCount++;
            }
        }

        return filledSlotsCount;
    }

}
