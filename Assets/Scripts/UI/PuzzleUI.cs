using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI stepCountText;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private Image progressBar;
    
    [Header("Settings")]
    [SerializeField] private float hintDisplayDuration = 3f;
    [SerializeField] private Color completedColor = Color.green;
    
    private DimensionPuzzle puzzle;
    private float hintTimer;

    private void Awake()
    {
        puzzle = GetComponent<DimensionPuzzle>();
        if (puzzle != null)
        {
            puzzle.onStepCompleted.AddListener(OnPuzzleStepCompleted);
            puzzle.onPuzzleCompleted.AddListener(OnPuzzleCompleted);
        }
    }

    private void Start()
    {
        UpdateUI(0);
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (hintTimer > 0)
        {
            hintTimer -= Time.deltaTime;
            if (hintTimer <= 0)
            {
                HideHint();
            }
        }
    }

    private void OnPuzzleStepCompleted(int step)
    {
        UpdateUI(step + 1);
        ShowHint("Step completed!");
    }

    private void OnPuzzleCompleted()
    {
        if (progressBar != null)
        {
            progressBar.color = completedColor;
        }
        ShowHint("Puzzle completed!");
    }

    private void UpdateUI(int currentStep)
    {
        if (stepCountText != null)
        {
            stepCountText.text = $"Step: {currentStep}";
        }

        if (progressBar != null && puzzle != null)
        {
            progressBar.fillAmount = currentStep / (float)puzzle.StepCount;
        }
    }

    public void ShowHint(string message)
    {
        if (hintText != null)
        {
            hintText.text = message;
            hintText.gameObject.SetActive(true);
            hintTimer = hintDisplayDuration;
        }
    }

    private void HideHint()
    {
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (puzzle != null)
        {
            puzzle.onStepCompleted.RemoveListener(OnPuzzleStepCompleted);
            puzzle.onPuzzleCompleted.RemoveListener(OnPuzzleCompleted);
        }
    }
} 