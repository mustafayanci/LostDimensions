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
    [SerializeField] private float transitionDuration = 1f;

    private int currentLevelIndex = -1;

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

    public void LoadFirstLevel()
    {
        LoadLevel(0);
    }

    public void LoadNextLevel()
    {
        LoadLevel(currentLevelIndex + 1);
    }

    public void ReloadCurrentLevel()
    {
        LoadLevel(currentLevelIndex);
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevelRoutine("MainMenu"));
        currentLevelIndex = -1;
    }

    private void LoadLevel(int index)
    {
        if (index >= 0 && index < levels.Length)
        {
            currentLevelIndex = index;
            var levelData = levels[currentLevelIndex];
            StartCoroutine(LoadLevelRoutine(levelData.sceneName));
            
            // Level ayarlarını uygula
            DimensionManager.Instance.SetAvailableDimensions(levelData.dimensionCount);
        }
        else
        {
            Debug.LogWarning($"Invalid level index: {index}");
        }
    }

    private System.Collections.IEnumerator LoadLevelRoutine(string sceneName)
    {
        // Geçiş efektini başlat
        UIManager.Instance.ShowTransition(true);
        yield return new WaitForSeconds(transitionDuration);

        // Sahneyi yükle
        var loadOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        // Oyuncuyu başlangıç pozisyonuna yerleştir
        if (currentLevelIndex >= 0)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = levels[currentLevelIndex].playerStartPosition;
            }
        }

        // Geçiş efektini bitir
        yield return new WaitForSeconds(transitionDuration);
        UIManager.Instance.ShowTransition(false);
    }

    public Vector3 GetCurrentLevelStartPosition()
    {
        return currentLevelIndex >= 0 ? levels[currentLevelIndex].playerStartPosition : Vector3.zero;
    }
} 