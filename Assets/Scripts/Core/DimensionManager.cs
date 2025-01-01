using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class DimensionManager : MonoBehaviour
{
    public static DimensionManager Instance { get; private set; }

    [Header("Dimension Settings")]
    [SerializeField] private int maxDimensions = 4;
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private Color[] dimensionColors;

    public int MaxDimensions => maxDimensions;
    public UnityEvent<int> onDimensionChanged = new UnityEvent<int>();

    private int currentDimension = 0;
    private int availableDimensions = 1;
    private List<IDimensionAware> dimensionAwareObjects = new List<IDimensionAware>();

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

    public void RegisterDimensionAware(IDimensionAware aware)
    {
        if (!dimensionAwareObjects.Contains(aware))
        {
            dimensionAwareObjects.Add(aware);
            aware.OnDimensionChanged(currentDimension);
        }
    }

    public void UnregisterDimensionAware(IDimensionAware aware)
    {
        dimensionAwareObjects.Remove(aware);
    }

    public void ChangeDimension(int dimensionId)
    {
        if (dimensionId >= 0 && dimensionId < availableDimensions)
        {
            currentDimension = dimensionId;
            
            foreach (var aware in dimensionAwareObjects)
            {
                if (aware != null)
                {
                    aware.OnDimensionChanged(currentDimension);
                }
            }

            onDimensionChanged?.Invoke(currentDimension);
            AudioManager.Instance.PlaySound($"DimensionChange_{currentDimension}");
        }
    }

    public void SetAvailableDimensions(int count)
    {
        availableDimensions = Mathf.Clamp(count, 1, maxDimensions);
        if (currentDimension >= availableDimensions)
        {
            ChangeDimension(0);
        }
    }

    public int GetCurrentDimension()
    {
        return currentDimension;
    }

    public Color GetDimensionColor(int dimensionId)
    {
        if (dimensionId >= 0 && dimensionId < dimensionColors.Length)
        {
            return dimensionColors[dimensionId];
        }
        return Color.white;
    }
}

// Boyut değişiminden etkilenecek nesneler için interface
public interface IDimensionAware
{
    void OnDimensionChanged(int dimensionId);
} 