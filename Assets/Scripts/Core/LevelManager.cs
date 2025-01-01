using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string[] levelScenes;
    
    private int currentLevelIndex = -1;
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

    public void LoadMainMenu()
    {
        currentLevelIndex = -1;
        LoadScene(mainMenuScene);
    }

    public void LoadFirstLevel()
    {
        if (levelScenes.Length > 0)
        {
            currentLevelIndex = 0;
            LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.LogError("No level scenes configured!");
        }
    }

    public void LoadNextLevel()
    {
        if (currentLevelIndex < levelScenes.Length - 1)
        {
            currentLevelIndex++;
            LoadLevel(currentLevelIndex);
        }
        else
        {
            // Oyun tamamlandı
            if (uiManager != null)
            {
                uiManager.ShowGameOver();
            }
        }
    }

    public void ReloadCurrentLevel()
    {
        if (currentLevelIndex >= 0 && currentLevelIndex < levelScenes.Length)
        {
            LoadLevel(currentLevelIndex);
        }
    }

    private void LoadLevel(int index)
    {
        if (index >= 0 && index < levelScenes.Length)
        {
            if (uiManager != null)
            {
                uiManager.ShowTransition(true);
            }
            LoadScene(levelScenes[index]);
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
} 