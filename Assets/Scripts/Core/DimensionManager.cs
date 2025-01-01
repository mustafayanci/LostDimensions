using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class DimensionManager : MonoBehaviour
{
    public static DimensionManager Instance { get; private set; }

    [SerializeField] private int startingDimension = 0;
    [SerializeField] private int maxDimensions = 4;
    public int MaxDimensions => maxDimensions;

    private int currentDimension;
    private int availableDimensions = 1;
    private readonly List<IDimensionAware> dimensionAwareObjects = new();

    public UnityEvent<int> onDimensionChanged = new UnityEvent<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentDimension = startingDimension;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterDimensionAware(IDimensionAware obj)
    {
        if (!dimensionAwareObjects.Contains(obj))
        {
            dimensionAwareObjects.Add(obj);
            obj.OnDimensionChanged(currentDimension);
        }
    }

    public void UnregisterDimensionAware(IDimensionAware obj)
    {
        dimensionAwareObjects.Remove(obj);
    }

    public void ChangeDimension(int newDimension)
    {
        if (newDimension >= 0 && newDimension < availableDimensions)
        {
            currentDimension = newDimension;
            foreach (var obj in dimensionAwareObjects)
            {
                obj.OnDimensionChanged(currentDimension);
            }
            onDimensionChanged?.Invoke(currentDimension);
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
} 