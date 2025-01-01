using UnityEngine;
using Interfaces;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private ParticleSystem hitEffect;
    
    private float damage;
    private bool hasHit;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<IPlayer>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            DestroyProjectile();
        }
        else if (other.CompareTag("Ground"))
        {
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        hasHit = true;
        if (hitEffect != null)
        {
            var effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect.gameObject, hitEffect.main.duration);
        }
        Destroy(gameObject);
    }
} 