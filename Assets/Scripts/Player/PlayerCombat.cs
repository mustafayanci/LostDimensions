using UnityEngine;
using Interfaces;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;

    private float attackTimer;
    private PlayerAnimator animator;

    private void Start()
    {
        animator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") && attackTimer <= 0)
        {
            Attack();
        }
    }

    private void Attack()
    {
        attackTimer = attackCooldown;
        animator?.PlayAttackAnimation();

        // Çevredeki düşmanları kontrol et
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            var enemyComponent = enemy.GetComponent<IEnemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(attackDamage);
            }
        }

        AudioManager.Instance.PlaySound("PlayerAttack");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
} 