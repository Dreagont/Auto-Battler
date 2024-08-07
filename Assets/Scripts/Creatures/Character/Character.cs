using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Character : MonoBehaviour
{
    public float health = 100f;
    public float attackSpeed = 1f;
    public float attackDamage = 50;
    public float armor = 5;
    private float attackCooldown = 0f;
    private List<Enemy> enemies;
    private Enemy currentTarget;
    public GameObject damageText;
    public TMP_Text popupText;
    public float increase;
    public Canvas canvas;
    bool isFirstHit = true;

    void Start()
    {
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
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
