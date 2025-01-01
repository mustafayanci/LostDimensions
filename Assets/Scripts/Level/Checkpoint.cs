using UnityEngine;
using Interfaces;

public class Checkpoint : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isActive;
    [SerializeField] private ParticleSystem activationEffect;
    [SerializeField] private SpriteRenderer checkpointSprite;
    
    [Header("Colors")]
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private Color activeColor = Color.green;

    private void Start()
    {
        if (checkpointSprite == null)
            checkpointSprite = GetComponent<SpriteRenderer>();
            
        UpdateVisuals();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var levelManager = FindFirstObjectByType<MonoBehaviour>() as ILevelManager;
            if (levelManager != null)
            {
                levelManager.AddCheckpoint(transform.position);
            }
        }
    }

    public void Activate()
    {
        isActive = true;
        UpdateVisuals();
        
        if (activationEffect != null)
            activationEffect.Play();
            
        LevelManager.Instance.AddCheckpoint(transform.position);
        AudioManager.Instance.PlaySound("CheckpointActivated");
    }

    private void UpdateVisuals()
    {
        if (checkpointSprite != null)
        {
            checkpointSprite.color = isActive ? activeColor : inactiveColor;
        }
    }

    public void Reset()
    {
        isActive = false;
        UpdateVisuals();
        
        if (activationEffect != null)
            activationEffect.Stop();
    }
} 