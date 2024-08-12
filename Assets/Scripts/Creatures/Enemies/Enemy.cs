using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float attackSpeed = 1f;
    public float armor;
    public float damage;
    private float attackCooldown = 0f;
    private Character character;
    public EnemyHealthBar healthBar;
    public GameObject damageText;
    public TMP_Text popupText;
    public float increase;
    public Canvas canvas;
    private EnemySpawner spawner;
    public EnemyTypeData enemyTypeData;
    private ActionsManager actionsManager;
    public int dropAmount = 1;
    public int goldDrop = 0;

    protected virtual void Start()
    {
        ApplyEnemyType();

        healthBar.UpdateBar(health, maxHealth);
        character = FindObjectOfType<Character>();
        spawner = FindObjectOfType<EnemySpawner>();
        actionsManager = FindObjectOfType<ActionsManager>();
        if (canvas == null)
        {
            GameObject canvasObject = GameObject.FindWithTag("Canvas");
            if (canvasObject != null)
            {
                canvas = canvasObject.GetComponent<Canvas>();
            }
            else
            {
                Debug.LogError("No canvas found with the 'DamageCanvas' tag.");
            }
        }
    }

    void ApplyEnemyType()
    {
        if (enemyTypeData != null)
        {
            int trueLevel = enemyTypeData.level - 1;
            float levelMultiplier = 1 + (trueLevel * 0.1f);

            attackSpeed = enemyTypeData.attackSpeed;
            armor = enemyTypeData.armor * levelMultiplier;
            maxHealth = enemyTypeData.maxHealth * levelMultiplier;
            damage = enemyTypeData.damage * levelMultiplier;
            goldDrop = Mathf.CeilToInt(enemyTypeData.gold * levelMultiplier);
            health = maxHealth;
        }
        else
        {
            Debug.LogError("EnemyTypeData is not assigned.");
        }
    }


    protected virtual void Update()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = 1f / attackSpeed;
        }
    }

    protected virtual void Attack()
    {
        if (character != null)
        {
            if (damage < 1f)
            {
                character.TakeDamage(1f);
            } else
            {
                character.TakeDamage(damage);
            }
        }
        else
        {
            Debug.LogWarning(name + " cannot find the character to attack.");
        }
    }

    public virtual void TakeDamage(float damage)
    {
            float amount = Mathf.Max(0, damage - (int)armor);
            if (amount < 1)
            {
                amount = 1;
            }
            health -= amount;
            healthBar.UpdateBar(health, maxHealth);

            GameObject damageTextInstance = Instantiate(damageText);
            RectTransform textTransform = damageTextInstance.GetComponent<RectTransform>();
            textTransform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, increase, 0);

            TMP_Text damageTextComponent = damageTextInstance.GetComponent<TMP_Text>();
            if (damageTextComponent != null)
            {
                damageTextComponent.text = amount.ToString();
            }
            else
            {
                Debug.LogError("TMP_Text component not found on damage text prefab");
            }

            textTransform.SetParent(canvas.transform);
            if (health <= 0f)
            {
                DropItems();
                Debug.LogError(name + " has died.");
                if (character != null)
                {
                    character.EnemyDestroyed(this);
                }
                if (spawner != null)
                {
                    spawner.OnEnemyKilled();
                }
                

                Destroy(gameObject);
            }  
    }

    public void SetLevel(int level)
    {
        enemyTypeData.level = level;
        ApplyEnemyType();
    }
    void DropItems()
    {
        if (enemyTypeData.dropItems != null && actionsManager != null)
        {
            foreach (ItemDrop itemDrop in enemyTypeData.dropItems)
            {
                if (itemDrop.item != null)
                {
                    int quantity = Random.Range(itemDrop.minQuantity, itemDrop.maxQuantity + 1);
                    for (int i = 0; i < quantity; i++)
                    {
                        actionsManager.PickupItem(itemDrop.item);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Either dropItems or actionsManager is null.");
        }

        actionsManager.GainGold(enemyTypeData.gold);
    }
}
