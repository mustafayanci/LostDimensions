using UnityEngine;

public class ChaserEnemy : EnemyBase
{
    [Header("Chaser Settings")]
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float stopDistance = 0.5f;
    
    protected override void OnPlayerDetected()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            rb.velocity = direction * chaseSpeed;
            // Düşmanın yönünü çevir
            transform.localScale = new Vector3(
                direction.x > 0 ? -1 : 1,
                1,
                1
            );
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    protected override void OnPlayerLost()
    {
        rb.velocity = Vector2.zero;
    }
} 