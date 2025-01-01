using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float damage = 20f;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float detectionRange = 5f;
    [SerializeField] protected LayerMask playerLayer;

    protected bool isDead;
    protected bool isActive = true;
    protected Transform player;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Update()
    {
        if (isDead || !isActive || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            OnPlayerDetected();
        }
    }

    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            AudioManager.Instance.PlaySound("EnemyHit");
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        AudioManager.Instance.PlaySound("EnemyDeath");
        Destroy(gameObject, 1f);
    }

    protected abstract void OnPlayerDetected();

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
} 