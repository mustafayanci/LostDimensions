using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDimensionAware
{
    [Header("Base Enemy Settings")]
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float detectionRange = 5f;
    [SerializeField] protected int[] activeDimensions;
    
    protected bool isActive = true;
    protected Transform player;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        if (!isActive) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            OnPlayerDetected();
        }
    }

    protected virtual void OnPlayerDetected()
    {
        // Alt sınıflar bu metodu override edecek
    }

    public virtual void OnDimensionChanged(int dimensionId)
    {
        isActive = System.Array.Exists(activeDimensions, d => d == dimensionId);
        gameObject.layer = isActive ? LayerMask.NameToLayer("Enemy") : LayerMask.NameToLayer("Inactive");
        spriteRenderer.enabled = isActive;
        rb.simulated = isActive;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var health = collision.gameObject.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
} 