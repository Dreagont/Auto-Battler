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
    bool isFirstHit = true;

    protected virtual void Start()
    {
        ApplyEnemyType();

        healthBar.UpdateBar(health, maxHealth);
        character = FindObjectOfType<Character>();
        spawner = FindObjectOfType<EnemySpawner>();


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
            float levelStat = (float)trueLevel * 10;
            attackSpeed = enemyTypeData.attackSpeed;
            armor = enemyTypeData.armor + enemyTypeData.armor * levelStat/100;
            maxHealth = enemyTypeData.maxHealth + enemyTypeData.maxHealth * levelStat / 100;
            damage = enemyTypeData.damage + enemyTypeData.damage * levelStat / 100;
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
            Debug.Log(name + " attacks character.");
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
        if (!isFirstHit)
        {
            float amount = Mathf.Max(0, damage - (int)armor);
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
        } else
        {
            isFirstHit = false;
            return;
        }
        
    }
}
