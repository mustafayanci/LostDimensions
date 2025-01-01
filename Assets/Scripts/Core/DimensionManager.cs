using UnityEngine;
using System.Collections.Generic;

public class DimensionManager : MonoBehaviour
{
    public static DimensionManager Instance { get; private set; }

    [Header("Dimension Settings")]
    [SerializeField] private int currentDimension = 0;
    [SerializeField] private int maxDimensions = 4;
    [SerializeField] private float dimensionChangeDelay = 0.5f;
    
    private bool canChangeDimension = true;
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
        if (!canChangeDimension || newDimension == currentDimension || 
            newDimension < 0 || newDimension >= maxDimensions)
            return;

        StartCoroutine(ChangeDimensionRoutine(newDimension));
    }

    private System.Collections.IEnumerator ChangeDimensionRoutine(int newDimension)
    {
        canChangeDimension = false;

        // Geçiş efektini başlat
        TransitionManager.Instance.PlayDimensionTransition(newDimension);
        AudioManager.Instance.PlaySound($"DimensionChange_{newDimension}");

        yield return new WaitForSeconds(dimensionChangeDelay);

        // Boyut değişimini uygula
        currentDimension = newDimension;
        foreach (var obj in dimensionAwareObjects)
        {
            obj.OnDimensionChanged(currentDimension);
        }

        // UI'ı güncelle
        UIManager.Instance.UpdateDimensionDisplay(currentDimension);

        yield return new WaitForSeconds(dimensionChangeDelay);
        
        canChangeDimension = true;
    }

    public void SetAvailableDimensions(int count)
    {
        maxDimensions = Mathf.Clamp(count, 1, 4);
        if (currentDimension >= maxDimensions)
        {
            ChangeDimension(0);
        }
    }

    public int GetCurrentDimension()
    {
        return currentDimension;
    }
}

// Boyut değişiminden etkilenecek nesneler için interface
public interface IDimensionAware
{
    void OnDimensionChanged(int dimensionId);
} 