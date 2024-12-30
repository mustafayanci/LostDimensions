using UnityEngine;
using UnityEngine.Events;

public abstract class PuzzleBase : MonoBehaviour, IDimensionAware
{
    [Header("Puzzle Base Settings")]
    [SerializeField] protected int[] activeDimensions;
    [SerializeField] protected bool isSolved;
    
    public UnityEvent onPuzzleSolved;
    public UnityEvent onPuzzleReset;
    
    protected virtual void Start()
    {
        ResetPuzzle();
    }

    public virtual void OnDimensionChanged(int dimensionId)
    {
        bool isActiveInDimension = System.Array.Exists(activeDimensions, d => d == dimensionId);
        gameObject.SetActive(isActiveInDimension);
        
        if (!isActiveInDimension)
        {
            ResetPuzzle();
        }
    }

    protected virtual void SolvePuzzle()
    {
        if (isSolved) return;
        
        isSolved = true;
        onPuzzleSolved?.Invoke();
    }

    protected virtual void ResetPuzzle()
    {
        if (!isSolved) return;
        
        isSolved = false;
        onPuzzleReset?.Invoke();
    }
} 