using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private bool isPaused;
    
    private UIManager uiManager;

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
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found in scene!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        if (Input.GetButtonDown("Pause") && CurrentState == GameState.Playing)
        {
            PauseGame();
        }
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
        LevelManager.Instance.LoadFirstLevel();
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
            Time.timeScale = 0;
            UIManager.Instance.ShowPauseMenu(true);
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
            Time.timeScale = 1;
            UIManager.Instance.ShowPauseMenu(false);
        }
    }

    public void GameOver()
    {
        if (CurrentState == GameState.GameOver) return;
        
        ChangeState(GameState.GameOver);
        StartCoroutine(GameOverRoutine());
    }

    private System.Collections.IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(gameOverDelay);
        UIManager.Instance.ShowGameOver();
        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        ChangeState(GameState.Playing);
        LevelManager.Instance.ReloadCurrentLevel();
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        ChangeState(GameState.MainMenu);
        LevelManager.Instance.LoadMainMenu();
    }

    private void ChangeState(GameState newState)
    {
        CurrentState = newState;
        onGameStateChanged?.Invoke(newState);
    }
} 