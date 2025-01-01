using UnityEngine;
using System.Collections.Generic;

public class DimensionManager : MonoBehaviour
{
    public static DimensionManager Instance { get; private set; }

    [SerializeField] private int startingDimension = 0;
    [SerializeField] private float transitionDuration = 1f;

    private int currentDimension;
    private readonly List<IDimensionAware> dimensionAwareObjects = new();

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
        if (currentDimension == newDimension) return;

        currentDimension = newDimension;
        foreach (var obj in dimensionAwareObjects)
        {
            obj.OnDimensionChanged(currentDimension);
        }
    }

    public int GetCurrentDimension()
    {
        return currentDimension;
    }
} 