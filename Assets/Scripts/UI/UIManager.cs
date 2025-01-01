using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IUIManager
{
    public static UIManager Instance { get; private set; }

    [Header("HUD Elements")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI dimensionText;
    [SerializeField] private GameObject[] dimensionIcons;

    [Header("Panels")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private CanvasGroup transitionPanel;

    [Header("Settings")]
    [SerializeField] private float transitionSpeed = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            HideAllPanels();
        }
        else
        {
            Destroy(gameObject);
        }
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
            hudPanel.SetActive(!show);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            hudPanel.SetActive(false);
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
        if (hudPanel != null) hudPanel.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (transitionPanel != null) transitionPanel.alpha = 0;
    }
} 