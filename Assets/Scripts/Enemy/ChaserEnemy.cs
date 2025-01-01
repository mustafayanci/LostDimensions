using UnityEngine;
using Interfaces;

public class ChaserEnemy : EnemyBase
{
    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;

    private float attackTimer;
    private bool isChasing;

    protected override void OnPlayerDetected()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        if (!isChasing) return;

        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;

        // Düşmanın yönünü çevir
        transform.localScale = new Vector3(
            direction.x > 0 ? -1 : 1,
            1,
            1
        );
    }

    private void Attack()
    {
        if (attackTimer <= 0)
        {
            var playerHealth = player.GetComponent<IPlayer>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                AudioManager.Instance.PlaySound("EnemyAttack");
            }
            attackTimer = attackCooldown;
        }
    }

    protected override void Update()
    {
        base.Update();
        
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
} 