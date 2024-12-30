using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 1f;
    
    public UnityEvent<int> onHealthChanged;
    public UnityEvent onPlayerDeath;
    
    private int currentHealth;
    private bool isInvincible;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        onHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    private void Die()
    {
        onPlayerDeath?.Invoke();
        // Ölüm animasyonu ve yeniden başlatma mantığı buraya gelecek
    }

    private System.Collections.IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        float elapsedTime = 0f;
        
        while (elapsedTime < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }
        
        spriteRenderer.enabled = true;
        isInvincible = false;
    }
} 