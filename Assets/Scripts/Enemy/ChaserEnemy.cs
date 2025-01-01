using UnityEngine;

public class ChaserEnemy : MonoBehaviour, IDimensionAware
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float detectionRange = 10f;
    
    [Header("Dimension Settings")]
    [SerializeField] private int[] activeDimensions;
    
    private Transform player;
    private Rigidbody2D rb;
    private bool isActive = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        DimensionManager.Instance.RegisterDimensionAware(this);
    }

    private void Update()
    {
        if (!isActive || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void OnDimensionChanged(int dimensionId)
    {
        isActive = System.Array.Exists(activeDimensions, d => d == dimensionId);
        gameObject.SetActive(isActive);
    }

    private void OnDestroy()
    {
        if (DimensionManager.Instance != null)
        {
            DimensionManager.Instance.UnregisterDimensionAware(this);
        }
    }
} 