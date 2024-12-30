using UnityEngine;

public class DimensionAudioController : MonoBehaviour, IDimensionAware
{
    [SerializeField] private string[] dimensionChangeSounds;
    
    public void OnDimensionChanged(int dimensionId)
    {
        // Boyut değişim sesini çal
        if (dimensionId < dimensionChangeSounds.Length)
        {
            AudioManager.Instance.PlaySound(dimensionChangeSounds[dimensionId]);
        }
        
        // Boyutun müziğini çal
        AudioManager.Instance.PlayDimensionMusic(dimensionId);
    }
} 