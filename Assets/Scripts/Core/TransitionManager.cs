using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [Header("Transition Elements")]
    [SerializeField] private CanvasGroup transitionPanel;
    [SerializeField] private Image transitionImage;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Dimension Transition")]
    [SerializeField] private Color[] dimensionColors;
    [SerializeField] private float dimensionTransitionDuration = 0.5f;

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

    public void PlayTransition(System.Action onTransitionComplete = null)
    {
        StartCoroutine(TransitionRoutine(onTransitionComplete));
    }

    public void PlayDimensionTransition(int dimensionId)
    {
        if (dimensionId < 0 || dimensionId >= dimensionColors.Length) return;
        
        Color targetColor = dimensionColors[dimensionId];
        StartCoroutine(DimensionTransitionRoutine(targetColor));
    }

    private System.Collections.IEnumerator TransitionRoutine(System.Action onComplete)
    {
        // Fade in
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float t = transitionCurve.Evaluate(elapsedTime / transitionDuration);
            transitionPanel.alpha = t;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transitionPanel.alpha = 1f;

        onComplete?.Invoke();

        // Fade out
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float t = transitionCurve.Evaluate(elapsedTime / transitionDuration);
            transitionPanel.alpha = 1f - t;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transitionPanel.alpha = 0f;
    }

    private System.Collections.IEnumerator DimensionTransitionRoutine(Color targetColor)
    {
        Color startColor = transitionImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < dimensionTransitionDuration)
        {
            float t = transitionCurve.Evaluate(elapsedTime / dimensionTransitionDuration);
            transitionImage.color = Color.Lerp(startColor, targetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transitionImage.color = targetColor;
    }
} 