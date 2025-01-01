using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerDimensionController : MonoBehaviour
{
    [Header("Dimension Settings")]
    [SerializeField] private float dimensionChangeCooldown = 1f;
    [SerializeField] private ParticleSystem dimensionChangeEffect;
    
    private bool canChangeDimension = true;
    private float cooldownTimer;
    private PlayerController playerController;
    private PlayerAnimator playerAnimator;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        if (!canChangeDimension)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                canChangeDimension = true;
            }
        }

        if (Input.GetButtonDown("DimensionChange") && canChangeDimension)
        {
            ChangeDimension();
        }
    }

    private void ChangeDimension()
    {
        int currentDimension = DimensionManager.Instance.GetCurrentDimension();
        int nextDimension = (currentDimension + 1) % DimensionManager.Instance.MaxDimensions;
        
        DimensionManager.Instance.ChangeDimension(nextDimension);
        
        // Efektleri oynat
        if (dimensionChangeEffect != null)
        {
            dimensionChangeEffect.Play();
        }

        // Cooldown'u başlat
        canChangeDimension = false;
        cooldownTimer = dimensionChangeCooldown;
    }
} 