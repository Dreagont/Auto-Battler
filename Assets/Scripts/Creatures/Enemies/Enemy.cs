using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 50;
    public int health = 50;
    public float attackSpeed = 1f;
    private float attackCooldown = 0f;
    private Character character;
    public EnemyHealthBar healthBar;

    void Start()
    {
        healthBar.UpdateBar(health, maxHealth);
        character = FindObjectOfType<Character>();
        Debug.Log(name + " started with health: " + health + " and attack speed: " + attackSpeed);
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = 1f / attackSpeed;
        }
    }

    void Attack()
    {
        if (character != null)
        {
            Debug.Log(name + " attacks character.");
            character.TakeDamage(5f); // Adjust damage as needed
        }
        else
        {
            Debug.LogWarning(name + " cannot find the character to attack.");
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        healthBar.UpdateBar(health, maxHealth);
        Debug.Log(name + " takes damage: " + amount + ", remaining health: " + health);
        if (health <= 0f)
        {
            Debug.LogError(name + " has died.");
            // Notify character of this enemy's destruction
            if (character != null)
            {
                character.EnemyDestroyed(this);
            }
            // Handle enemy death
            Destroy(gameObject);
        }
    }
}
