using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        DimensionManager.Instance.RegisterDimensionAware(this);
    }

    public void OnDimensionChanged(int dimensionId)
    {
        var state = System.Array.Find(dimensionStates, s => s.dimensionId == dimensionId);
        
        if (state != null)
        {
            gameObject.SetActive(state.isActive);
            if (state.isActive)
            {
                spriteRenderer.color = state.platformColor;
                platformCollider.isTrigger = !state.isSolid;
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