using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [System.Serializable]
    public class LevelData
    {
        public string sceneName;
        public int dimensionCount;
        public Vector3 playerStartPosition;
    }

    [Header("Level Settings")]
    [SerializeField] private LevelData[] levels;
    [SerializeField] private float levelTransitionTime = 1f;
    
    [Header("Checkpoint Settings")]
    [SerializeField] private float checkpointActivationDelay = 0.5f;
    
    private int currentLevelIndex;
    private List<Vector3> checkpoints = new List<Vector3>();
    private bool isTransitioning;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLevel();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeLevel()
    {
        currentLevelIndex = 0;
        checkpoints.Clear();
        isTransitioning = false;
    }

    public void LoadNextLevel()
    {
        if (isTransitioning) return;
        
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Length)
        {
            // Oyun bitti
            Debug.Log("Game Completed!");
            return;
        }

        StartCoroutine(LoadLevelRoutine(levels[currentLevelIndex].sceneName));
    }

    private System.Collections.IEnumerator LoadLevelRoutine(string sceneName)
    {
        isTransitioning = true;
        
        // Geçiş efektini başlat
        UIManager.Instance.ShowTransition(true);
        
        yield return new WaitForSeconds(levelTransitionTime);

        // Yeni sahneyi yükle
        var operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }

        // Level'ı hazırla
        SetupLevel(levels[currentLevelIndex]);
        
        // Geçiş efektini bitir
        UIManager.Instance.ShowTransition(false);
        
        isTransitioning = false;
    }

    private void SetupLevel(LevelData levelData)
    {
        // Oyuncuyu başlangıç pozisyonuna yerleştir
        if (GameManager.Instance.playerPrefab != null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                player = Instantiate(GameManager.Instance.playerPrefab, levelData.playerStartPosition, Quaternion.identity);
            }
            else
            {
                player.transform.position = levelData.playerStartPosition;
            }
        }

        // Checkpoint'leri temizle
        checkpoints.Clear();
        
        // Boyut sayısını ayarla
        DimensionManager.Instance.SetAvailableDimensions(levelData.dimensionCount);
    }

    public void AddCheckpoint(Vector3 position)
    {
        if (!checkpoints.Contains(position))
        {
            checkpoints.Add(position);
            GameManager.Instance.SetCheckpoint(position);
            StartCoroutine(ShowCheckpointActivation(position));
        }
    }

    private System.Collections.IEnumerator ShowCheckpointActivation(Vector3 position)
    {
        // Checkpoint efektini göster
        // TODO: Checkpoint efekti ekle
        
        yield return new WaitForSeconds(checkpointActivationDelay);
    }

    public Vector3 GetLastCheckpoint()
    {
        if (checkpoints.Count > 0)
            return checkpoints[checkpoints.Count - 1];
            
        return levels[currentLevelIndex].playerStartPosition;
    }
} 