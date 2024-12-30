using UnityEngine;
using System.Collections.Generic;

public class DimensionManager : MonoBehaviour
{
    public static DimensionManager Instance { get; private set; }
    
    [System.Serializable]
    public class DimensionData
    {
        public int dimensionId;
        public Vector2 gravity;
        public Color ambientColor;
        public float timeScale;
    }

    [SerializeField] private List<DimensionData> dimensions;
    [SerializeField] private float transitionDuration = 1f;
    
    private int currentDimensionId;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeDimension(int newDimensionId)
    {
        if (newDimensionId == currentDimensionId) return;
        
        DimensionData newDimension = dimensions.Find(d => d.dimensionId == newDimensionId);
        if (newDimension == null) return;

        StartCoroutine(TransitionToDimension(newDimension));
    }

    private System.Collections.IEnumerator TransitionToDimension(DimensionData newDimension)
    {
        // Geçiş efektini başlat
        float elapsedTime = 0;
        DimensionData oldDimension = dimensions.Find(d => d.dimensionId == currentDimensionId);

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            
            // Yerçekimi değişimi
            Physics2D.gravity = Vector2.Lerp(oldDimension.gravity, newDimension.gravity, t);
            
            // Renk değişimi
            RenderSettings.ambientLight = Color.Lerp(oldDimension.ambientColor, newDimension.ambientColor, t);
            
            // Zaman ölçeği değişimi
            Time.timeScale = Mathf.Lerp(oldDimension.timeScale, newDimension.timeScale, t);
            
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        currentDimensionId = newDimension.dimensionId;
        
        // Boyut değişimi olayını bildir
        OnDimensionChanged(newDimension.dimensionId);
    }

    private void OnDimensionChanged(int dimensionId)
    {
        // Boyut değişimi olayını dinleyen tüm nesneleri bilgilendir
        var dimensionObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDimensionAware>();
        foreach (var obj in dimensionObjects)
        {
            obj.OnDimensionChanged(dimensionId);
        }
    }
}

// Boyut değişiminden etkilenecek nesneler için interface
public interface IDimensionAware
{
    void OnDimensionChanged(int dimensionId);
} 