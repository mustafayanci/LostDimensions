using UnityEngine;

public class PlayerDimensionController : MonoBehaviour
{
    [SerializeField] private KeyCode[] dimensionKeys = {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4
    };

    private void Update()
    {
        for (int i = 0; i < dimensionKeys.Length && i < DimensionManager.Instance.MaxDimensions; i++)
        {
            if (Input.GetKeyDown(dimensionKeys[i]))
            {
                DimensionManager.Instance.ChangeDimension(i);
                break;
            }
        }
    }
} 