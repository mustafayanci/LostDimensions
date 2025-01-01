using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("Ranged Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float projectileSpeed = 10f;

    private float shootTimer;

    protected override void OnPlayerDetected()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            
            var rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }

            var projectileComponent = projectile.GetComponent<EnemyProjectile>();
            if (projectileComponent != null)
            {
                projectileComponent.Initialize(damage);
            }

            shootTimer = shootInterval;
            AudioManager.Instance.PlaySound("EnemyShoot");
        }
    }
} 