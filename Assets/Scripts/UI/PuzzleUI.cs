using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI puzzleNameText;
    [SerializeField] private TextMeshProUGUI puzzleStatusText;
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject puzzleHintPanel;
    
    private PuzzleBase currentPuzzle;

    public void Initialize(PuzzleBase puzzle, string puzzleName)
    {
        currentPuzzle = puzzle;
        puzzleNameText.text = puzzleName;
        
        currentPuzzle.onPuzzleSolved.AddListener(OnPuzzleSolved);
        currentPuzzle.onPuzzleReset.AddListener(OnPuzzleReset);
        
        UpdateUI("In Progress");
    }

    private void OnPuzzleSolved()
    {
        UpdateUI("Solved!");
        StartCoroutine(HideAfterDelay(2f));
    }

    private void OnPuzzleReset()
    {
        UpdateUI("Reset");
    }

    private void UpdateUI(string status)
    {
        puzzleStatusText.text = status;
    }

    public void ShowHint()
    {
        puzzleHintPanel.SetActive(true);
        StartCoroutine(HideHintAfterDelay(5f));
    }

    private System.Collections.IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator HideHintAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        puzzleHintPanel.SetActive(false);
    }
} 