using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float invincibilityDuration = 1f;
    
    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent<float> onHealthChanged;
    
    private bool isInvincible;
    private float invincibilityTimer;

    private void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth / maxHealth);
    }

    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        onHealthChanged?.Invoke(currentHealth / maxHealth);
        
        AudioManager.Instance.PlaySound("PlayerHurt");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartInvincibility();
        }
    }

    public void RestoreHealth(float amount = -1)
    {
        if (amount < 0) amount = maxHealth;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        onHealthChanged?.Invoke(currentHealth / maxHealth);
        
        AudioManager.Instance.PlaySound("HealthPickup");
    }

    private void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    private void Die()
    {
        onDeath?.Invoke();
        GameManager.Instance.GameOver();
        AudioManager.Instance.PlaySound("PlayerDeath");
        gameObject.SetActive(false);
    }
} 