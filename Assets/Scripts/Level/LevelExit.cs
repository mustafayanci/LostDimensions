using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isLocked = true;
    [SerializeField] private string requiredPuzzleTag = "MainPuzzle";
    
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer exitSprite;
    [SerializeField] private Color lockedColor = Color.red;
    [SerializeField] private Color unlockedColor = Color.green;
    [SerializeField] private ParticleSystem unlockEffect;

    private void Start()
    {
        if (exitSprite == null)
            exitSprite = GetComponent<SpriteRenderer>();
            
        UpdateVisuals();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isLocked) return;
        
        if (other.CompareTag("Player"))
        {
            CompleteLevel();
        }
    }

    public void Unlock()
    {
        isLocked = false;
        UpdateVisuals();
        
        if (unlockEffect != null)
            unlockEffect.Play();
            
        AudioManager.Instance.PlaySound("ExitUnlocked");
    }

    private void UpdateVisuals()
    {
        if (exitSprite != null)
        {
            exitSprite.color = isLocked ? lockedColor : unlockedColor;
        }
    }

    private void CompleteLevel()
    {
        // Level geçiş efektini başlat
        AudioManager.Instance.PlaySound("LevelComplete");
        LevelManager.Instance.LoadNextLevel();
    }

    // Bulmaca tamamlandığında çağrılacak
    public void OnPuzzleCompleted(GameObject puzzle)
    {
        if (puzzle.CompareTag(requiredPuzzleTag))
        {
            Unlock();
        }
    }
} 