using UnityEngine;
using UnityEngine.Events;
using Interfaces;

public class DimensionPuzzle : MonoBehaviour, IPuzzle
{
    [System.Serializable]
    public class PuzzleStep
    {
        public int requiredDimension;
        public GameObject targetObject;
        public bool isCompleted;
    }

    [Header("Puzzle Settings")]
    [SerializeField] private PuzzleStep[] puzzleSteps;
    [SerializeField] private bool requireSequentialOrder = true;
    [SerializeField] private ParticleSystem completionEffect;
    
    [Header("Events")]
    public UnityEvent onPuzzleCompleted;
    public UnityEvent<int> onStepCompleted;

    private int currentStepIndex;
    private bool isPuzzleCompleted;

    private void Start()
    {
        DimensionManager.Instance.onDimensionChanged.AddListener(OnDimensionChanged);
    }

    private void OnDimensionChanged(int newDimension)
    {
        if (isPuzzleCompleted) return;

        if (requireSequentialOrder)
        {
            CheckSequentialStep(newDimension);
        }
        else
        {
            CheckAnyStep(newDimension);
        }
    }

    private void CheckSequentialStep(int dimension)
    {
        if (currentStepIndex >= puzzleSteps.Length) return;

        var currentStep = puzzleSteps[currentStepIndex];
        if (dimension == currentStep.requiredDimension)
        {
            CompleteStep(currentStep);
        }
    }

    private void CheckAnyStep(int dimension)
    {
        bool allCompleted = true;
        for (int i = 0; i < puzzleSteps.Length; i++)
        {
            if (!puzzleSteps[i].isCompleted && dimension == puzzleSteps[i].requiredDimension)
            {
                CompleteStep(puzzleSteps[i]);
            }
            allCompleted &= puzzleSteps[i].isCompleted;
        }

        if (allCompleted)
        {
            CompletePuzzle();
        }
    }

    private void CompleteStep(PuzzleStep step)
    {
        step.isCompleted = true;
        if (step.targetObject != null)
        {
            step.targetObject.SetActive(true);
        }
        
        onStepCompleted?.Invoke(currentStepIndex);
        AudioManager.Instance.PlaySound("PuzzleStep");

        currentStepIndex++;
        if (currentStepIndex >= puzzleSteps.Length)
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        if (isPuzzleCompleted) return;
        
        isPuzzleCompleted = true;
        if (completionEffect != null)
        {
            completionEffect.Play();
        }
        
        onPuzzleCompleted?.Invoke();
        AudioManager.Instance.PlaySound("PuzzleComplete");
    }

    private void OnDestroy()
    {
        if (DimensionManager.Instance != null)
        {
            DimensionManager.Instance.onDimensionChanged.RemoveListener(OnDimensionChanged);
        }
    }

    public int StepCount => currentStepIndex;
} 