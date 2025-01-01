using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DimensionAudioController : MonoBehaviour, IDimensionAware
{
    private AudioSource audioSource;
    private int currentDimension;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        DimensionManager.Instance.RegisterDimensionAware(this);
    }

    public void OnDimensionChanged(int dimensionId)
    {
        if (currentDimension != dimensionId)
        {
            currentDimension = dimensionId;
            AudioManager.Instance.PlayDimensionMusic(dimensionId);
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