using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD Elements")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image[] healthIcons;
    [SerializeField] private TextMeshProUGUI dimensionText;
    
    [Header("Menu Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject dimensionTransitionPanel;

    [Header("Animation")]
    [SerializeField] private Animator transitionAnimator;
    
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

    public void UpdateHealth(int currentHealth)
    {
        healthText.text = $"HP: {currentHealth}";
        
        for (int i = 0; i < healthIcons.Length; i++)
        {
            healthIcons[i].enabled = i < currentHealth;
        }
    }

    public void UpdateDimensionDisplay(int dimensionId)
    {
        dimensionText.text = $"Dimension: {dimensionId}";
        StartCoroutine(PlayDimensionTransition());
    }

    private System.Collections.IEnumerator PlayDimensionTransition()
    {
        dimensionTransitionPanel.SetActive(true);
        transitionAnimator.SetTrigger("StartTransition");
        
        yield return new WaitForSeconds(1f);
        
        dimensionTransitionPanel.SetActive(false);
    }

    public void ShowPauseMenu(bool show)
    {
        pausePanel.SetActive(show);
        Time.timeScale = show ? 0f : 1f;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        hudPanel.SetActive(false);
    }
} 