using UnityEngine;

public class TimedPuzzle : PuzzleBase
{
    [Header("Timed Puzzle Settings")]
    [SerializeField] private float timeLimit = 10f;
    [SerializeField] private bool autoStart;
    
    private float remainingTime;
    private bool isRunning;

    protected override void Start()
    {
        base.Start();
        remainingTime = timeLimit;
        
        if (autoStart)
        {
            StartTimer();
        }
    }

    private void Update()
    {
        if (!isRunning) return;

        remainingTime -= Time.deltaTime;
        
        if (remainingTime <= 0)
        {
            ResetPuzzle();
            remainingTime = timeLimit;
            isRunning = false;
        }
    }

    public void StartTimer()
    {
        if (isRunning) return;
        
        isRunning = true;
        remainingTime = timeLimit;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    protected override void ResetPuzzle()
    {
        base.ResetPuzzle();
        remainingTime = timeLimit;
        isRunning = false;
    }
} 