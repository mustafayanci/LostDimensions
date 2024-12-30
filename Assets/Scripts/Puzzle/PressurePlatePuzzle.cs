using UnityEngine;

public class PressurePlatePuzzle : PuzzleBase
{
    [Header("Pressure Plate Settings")]
    [SerializeField] private float requiredWeight = 1f;
    [SerializeField] private float activationDelay = 0.5f;
    
    private float currentWeight;
    private float timePressed;
    private bool isPressed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
        {
            currentWeight += other.attachedRigidbody.mass;
            CheckWeight();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
        {
            currentWeight -= other.attachedRigidbody.mass;
            CheckWeight();
        }
    }

    private void CheckWeight()
    {
        bool wasPressed = isPressed;
        isPressed = currentWeight >= requiredWeight;

        if (isPressed != wasPressed)
        {
            timePressed = isPressed ? Time.time : 0f;
        }

        if (isPressed && Time.time - timePressed >= activationDelay)
        {
            SolvePuzzle();
        }
        else if (!isPressed)
        {
            ResetPuzzle();
        }
    }
} 