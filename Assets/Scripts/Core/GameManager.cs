using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MainMenu, Playing, Paused, GameOver }
    
    [Header("Game Settings")]
    [SerializeField] private float gameOverDelay = 2f;
    
    public GameState CurrentState { get; private set; }
    public UnityEvent<GameState> onGameStateChanged = new UnityEvent<GameState>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupGame()
    {
        Application.targetFrameRate = 60;
        CurrentState = GameState.MainMenu;
    }

    private void Update()
    {
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