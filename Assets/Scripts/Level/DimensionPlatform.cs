using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DimensionPlatform : MonoBehaviour, IDimensionAware
{
    [System.Serializable]
    public class DimensionState
    {
        public int dimensionId;
        public bool isActive = true;
        public bool isSolid = true;
        public Color platformColor = Color.white;
    }

    [SerializeField] private DimensionState[] dimensionStates;
    
    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;
    private DimensionState currentState;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        DimensionManager.Instance.RegisterDimensionAware(this);
    }

    public void OnDimensionChanged(int dimensionId)
    {
        currentState = System.Array.Find(dimensionStates, state => state.dimensionId == dimensionId);
        
        if (currentState != null)
        {
            gameObject.SetActive(currentState.isActive);
            if (currentState.isActive)
            {
                spriteRenderer.color = currentState.platformColor;
                platformCollider.isTrigger = !currentState.isSolid;
            }
        }
        else
        {
            gameObject.SetActive(false);
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