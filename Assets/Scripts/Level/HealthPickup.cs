using UnityEngine;

public class HealthPickup : MonoBehaviour, IDimensionAware
{
    [SerializeField] private float healAmount = 25f;
    [SerializeField] private int[] activeDimensions;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RestoreHealth(healAmount);
                Destroy(gameObject);
            }
        }
    }

    public void OnDimensionChanged(int dimensionId)
    {
        bool isActive = System.Array.Exists(activeDimensions, d => d == dimensionId);
        gameObject.SetActive(isActive);
    }
} 