using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game States")]
    public bool isGamePaused;
    public bool isGameOver;
    
    [Header("Player Reference")]
    public GameObject playerPrefab;
    private GameObject currentPlayer;
    private Vector3 lastCheckpoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        isGamePaused = false;
        isGameOver = false;
        lastCheckpoint = Vector3.zero;
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        UIManager.Instance.ShowPauseMenu(true);
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        UIManager.Instance.ShowPauseMenu(false);
    }

    public void GameOver()
    {
        isGameOver = true;
        UIManager.Instance.ShowGameOver();
    }

    public void RestartLevel()
    {
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpoint = position;
    }

    public void RespawnPlayer()
    {
        if (currentPlayer != null)
        {
            currentPlayer.transform.position = lastCheckpoint;
            var health = currentPlayer.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.RestoreHealth();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
} 