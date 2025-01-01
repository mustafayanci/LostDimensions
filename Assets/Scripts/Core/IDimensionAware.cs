using UnityEngine;

/// <summary>
/// Boyut değişiminden etkilenen nesneler için interface
/// </summary>
public interface IDimensionAware
{
    /// <summary>
    /// Boyut değiştiğinde çağrılır
    /// </summary>
    /// <param name="dimensionId">Yeni boyut ID'si</param>
    void OnDimensionChanged(int dimensionId);
} 