using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private CanvasGroup transitionPanel;

    [Header("HUD Elements")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI dimensionText;
    [SerializeField] private GameObject[] dimensionIcons;

    [Header("Settings")]
    [SerializeField] private float transitionSpeed = 2f;
    
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

    private void Start()
    {
        HideAllPanels();
    }

    public void UpdateHealthBar(float healthPercentage)
    {
        if (healthBar != null)
        {
            healthBar.value = healthPercentage;
        }
    }

    public void UpdateDimensionDisplay(int dimensionId)
    {
        if (dimensionText != null)
        {
            string[] dimensionNames = { "Normal", "Fire", "Nature", "Shadow" };
            dimensionText.text = dimensionId < dimensionNames.Length ? dimensionNames[dimensionId] : "Unknown";
        }

        // Dimension ikonlarını güncelle
        for (int i = 0; i < dimensionIcons.Length; i++)
        {
            if (dimensionIcons[i] != null)
            {
                dimensionIcons[i].SetActive(i == dimensionId);
            }
        }
    }

    public void ShowPauseMenu(bool show)
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(show);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void ShowTransition(bool show)
    {
        if (transitionPanel != null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeTransition(show ? 1 : 0));
        }
    }

    private System.Collections.IEnumerator FadeTransition(float targetAlpha)
    {
        while (!Mathf.Approximately(transitionPanel.alpha, targetAlpha))
        {
            transitionPanel.alpha = Mathf.MoveTowards(
                transitionPanel.alpha,
                targetAlpha,
                Time.deltaTime * transitionSpeed
            );
            yield return null;
        }
    }

    private void HideAllPanels()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (transitionPanel != null) transitionPanel.alpha = 0;
    }

    // UI Button Events
    public void OnResumeClicked()
    {
        GameManager.Instance.ResumeGame();
    }

    public void OnRestartClicked()
    {
        GameManager.Instance.RestartLevel();
    }

    public void OnQuitClicked()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
} 