using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    public float health = 100f;
    public float attackSpeed = 1f;
    private float attackCooldown = 0f;
    private List<Enemy> enemies;
    private Enemy currentTarget;

    void Start()
    {
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        Debug.Log("Character started with health: " + health + " and attack speed: " + attackSpeed);
        SelectNewTarget();
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
        // Remove null enemies from the list
        enemies.RemoveAll(enemy => enemy == null);

        if (currentTarget == null)
        {
            SelectNewTarget();
        }

        if (currentTarget != null)
        {
            Debug.Log("Character attacks enemy: " + currentTarget.name);
            currentTarget.TakeDamage(10); // Adjust damage as needed
        }
        else
        {
            Debug.LogWarning("No enemies found to attack.");
        }
    }

    void SelectNewTarget()
    {
        if (enemies.Count > 0)
        {
            int randomIndex = Random.Range(0, enemies.Count);
            currentTarget = enemies[randomIndex];
            Debug.Log("Character selected new target: " + currentTarget.name);
        }
        else
        {
            currentTarget = null;
            Debug.LogWarning("No enemies available to select as new target.");
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Character takes damage: " + amount + ", remaining health: " + health);
        if (health <= 0f)
        {
            Debug.LogError("Character has died.");
            // Handle character death
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
}
