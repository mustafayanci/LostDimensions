using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayers;
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem attackEffect;
    [SerializeField] private Transform attackPoint;

    private bool canAttack = true;
    private float cooldownTimer;

    private void Update()
    {
        if (!canAttack)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                canAttack = true;
            }
        }

        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Saldırı animasyonunu oynat
        GetComponent<PlayerAnimator>()?.PlayAttackAnimation();

        // Efekti oynat
        if (attackEffect != null)
        {
            attackEffect.Play();
        }

        // Çevredeki düşmanları bul ve hasar ver
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyBase>()?.TakeDamage(attackDamage);
        }

        // Cooldown'u başlat
        canAttack = false;
        cooldownTimer = attackCooldown;
        
        AudioManager.Instance.PlaySound("PlayerAttack");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
} 