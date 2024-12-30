using UnityEngine;

public class PatrolEnemy : EnemyBase
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTime = 1f;
    
    private int currentPoint = 0;
    private float waitCounter = 0f;
    private bool isWaiting = false;

    protected override void Update()
    {
        base.Update();
        if (!isActive || patrolPoints.Length == 0) return;
        
        if (isWaiting)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
            {
                isWaiting = false;
                waitCounter = 0f;
            }
            return;
        }

        Vector2 targetPosition = patrolPoints[currentPoint].position;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        
        rb.linearVelocity = direction * moveSpeed;

        // Düşmanın yönünü çevir
        transform.localScale = new Vector3(
            direction.x > 0 ? -1 : 1,
            1,
            1
        );

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            isWaiting = true;
        }
    }

    protected override void OnPlayerDetected()
    {
        // Oyuncu tespit edildiğinde devriye gezmeyi durdur ve oyuncuya saldır
        isWaiting = false;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * (moveSpeed * 1.5f);
    }
} 