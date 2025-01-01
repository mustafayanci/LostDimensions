using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private bool isPaused;
    
    private IUIManager uiManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        uiManager = FindFirstObjectByType<MonoBehaviour>() as IUIManager;
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found in scene!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        if (uiManager != null)
        {
            uiManager.ShowPauseMenu(true);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        if (uiManager != null)
        {
            uiManager.ShowPauseMenu(false);
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            LevelManager.Instance.LoadFirstLevel();
        }
    }
} 