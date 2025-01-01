using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDimensionAware
{
    [Header("Base Enemy Settings")]
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float detectionRange = 10f;
    [SerializeField] protected int[] activeDimensions;

    protected Transform player;
    protected Rigidbody2D rb;
    protected bool isActive = true;
    protected bool isDead;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        DimensionManager.Instance.RegisterDimensionAware(this);
    }

    protected virtual void Update()
    {
        if (!isActive || isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            OnPlayerDetected();
        }
        else
        {
            OnPlayerLost();
        }
    }

    protected abstract void OnPlayerDetected();
    protected virtual void OnPlayerLost() { }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        // Ölüm efektleri ve sesleri burada çalınacak
        AudioManager.Instance.PlaySound("EnemyDeath");
        Destroy(gameObject);
    }

    public virtual void OnDimensionChanged(int dimensionId)
    {
        isActive = System.Array.Exists(activeDimensions, d => d == dimensionId);
        gameObject.SetActive(isActive);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    protected virtual void OnDestroy()
    {
        if (DimensionManager.Instance != null)
        {
            DimensionManager.Instance.UnregisterDimensionAware(this);
        }
    }
} 