using UnityEngine;
using System.Collections.Generic;

public class SequencePuzzle : PuzzleBase
{
    [Header("Sequence Settings")]
    [SerializeField] private int[] correctSequence;
    [SerializeField] private float resetDelay = 2f;
    
    private List<int> currentSequence = new List<int>();
    private bool isWaitingForReset;

    public void AddToSequence(int value)
    {
        if (isSolved || isWaitingForReset) return;

        currentSequence.Add(value);
        
        if (CheckSequence())
        {
            if (currentSequence.Count == correctSequence.Length)
            {
                SolvePuzzle();
            }
        }
        else
        {
            StartCoroutine(DelayedReset());
        }
    }

    private bool CheckSequence()
    {
        for (int i = 0; i < currentSequence.Count; i++)
        {
            if (currentSequence[i] != correctSequence[i])
            {
                return false;
            }
        }
        return true;
    }

    private System.Collections.IEnumerator DelayedReset()
    {
        isWaitingForReset = true;
        yield return new WaitForSeconds(resetDelay);
        ResetPuzzle();
        isWaitingForReset = false;
    }

    protected override void ResetPuzzle()
    {
        base.ResetPuzzle();
        currentSequence.Clear();
    }
} 