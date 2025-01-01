using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour, IDimensionAware
{
    [Header("Settings")]
    [SerializeField] private float activationDelay = 0.5f;
    [SerializeField] private bool stayPressed;
    [SerializeField] private int[] activeDimensions;
    
    [Header("Visual")]
    [SerializeField] private SpriteRenderer plateSprite;
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Color inactiveColor = Color.red;
    [SerializeField] private ParticleSystem activationEffect;

    [Header("Events")]
    public UnityEvent onPlatePressed;
    public UnityEvent onPlateReleased;

    private bool isPressed;
    private float pressTimer;

    private void Start()
    {
        if (plateSprite == null)
            plateSprite = GetComponent<SpriteRenderer>();

        UpdateVisuals();
        DimensionManager.Instance.RegisterDimensionAware(this);
    }

    private void Update()
    {
        if (isPressed && !stayPressed)
        {
            pressTimer -= Time.deltaTime;
            if (pressTimer <= 0)
            {
                Release();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PressurePlateObject"))
        {
            Press();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!stayPressed && (other.CompareTag("Player") || other.CompareTag("PressurePlateObject")))
        {
            pressTimer = activationDelay;
        }
    }

    private void Press()
    {
        if (isPressed) return;

        isPressed = true;
        UpdateVisuals();
        
        if (activationEffect != null)
            activationEffect.Play();

        onPlatePressed?.Invoke();
        AudioManager.Instance.PlaySound("PlatePressed");
    }

    private void Release()
    {
        if (!isPressed) return;

        isPressed = false;
        UpdateVisuals();
        onPlateReleased?.Invoke();
        AudioManager.Instance.PlaySound("PlateReleased");
    }

    private void UpdateVisuals()
    {
        if (plateSprite != null)
        {
            plateSprite.color = isPressed ? activeColor : inactiveColor;
        }
    }

    public void OnDimensionChanged(int dimensionId)
    {
        bool isActive = System.Array.Exists(activeDimensions, d => d == dimensionId);
        gameObject.SetActive(isActive);
        
        if (!isActive && isPressed)
        {
            Release();
        }
    }

    private void OnDestroy()
    {
        if (DimensionManager.Instance != null)
        {
            DimensionManager.Instance.UnregisterDimensionAware(this);
        }
    }
} 