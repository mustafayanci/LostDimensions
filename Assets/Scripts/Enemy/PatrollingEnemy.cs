using UnityEngine;

public class PatrollingEnemy : EnemyBase
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float waitTime = 1f;
    
    private int currentPointIndex;
    private float waitTimer;
    private bool isWaiting;

    protected override void Start()
    {
        base.Start();
        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points assigned to " + gameObject.name);
        }
    }

    protected override void Update()
    {
        base.Update();
        
        if (!isActive || isDead || patrolPoints.Length == 0) return;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
            }
            return;
        }

        Patrol();
    }

    protected override void OnPlayerDetected()
    {
        // Player tespit edildiğinde yapılacak işlemler
        // Örneğin: Devriye gezmeyi durdur ve oyuncuya saldır
    }

    private void Patrol()
    {
        Vector2 targetPoint = patrolPoints[currentPointIndex].position;
        Vector2 moveDirection = (targetPoint - (Vector2)transform.position).normalized;
        
        rb.linearVelocity = moveDirection * patrolSpeed;

        // Düşmanın yönünü çevir
        transform.localScale = new Vector3(
            moveDirection.x > 0 ? -1 : 1,
            1,
            1
        );

        if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            isWaiting = true;
            waitTimer = waitTime;
            rb.linearVelocity = Vector2.zero;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Gizmos.color = Color.blue;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] != null)
            {
                Gizmos.DrawWireSphere(patrolPoints[i].position, 0.3f);
                if (i < patrolPoints.Length - 1 && patrolPoints[i + 1] != null)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                }
            }
        }
    }
} 