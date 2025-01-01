using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Player;

public class SceneSetup
{
    [MenuItem("Tools/Setup/Create Initial Scene")]
    public static void CreateMainScene()
    {
        // Ensure project structure exists
        ProjectSetup.CreateFolderStructure();

        // Save current scene
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        // Create and save new scene
        string scenePath = "Assets/Scenes/MainScene.unity";
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        
        // Create essential objects
        CreateGameManagers();
        CreatePlayer();
        CreateEnvironment();
        SetupCamera();
        SetupUI();

        // Save scene
        EnsureDirectoryExists("Assets/Scenes");
        EditorSceneManager.SaveScene(scene, scenePath);
        
        // Set as first scene in build settings
        EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
        {
            new EditorBuildSettingsScene(scenePath, true)
        };

        Debug.Log("Main scene created successfully!");
    }

    private static void CreateGameManagers()
    {
        var managers = new GameObject("--- MANAGERS ---");
        
        // Game Manager
        var gameManager = new GameObject("GameManager");
        gameManager.AddComponent<GameManager>();
        gameManager.transform.SetParent(managers.transform);

        // Audio Manager
        var audioManager = new GameObject("AudioManager");
        audioManager.AddComponent<AudioManager>();
        audioManager.transform.SetParent(managers.transform);

        // Level Manager
        var levelManager = new GameObject("LevelManager");
        levelManager.AddComponent<LevelManager>();
        levelManager.transform.SetParent(managers.transform);

        // Dimension Manager
        var dimensionManager = new GameObject("DimensionManager");
        dimensionManager.AddComponent<DimensionManager>();
        dimensionManager.transform.SetParent(managers.transform);
    }

    private static void CreatePlayer()
    {
        var player = new GameObject("Player");
        player.tag = "Player";
        
        // Player bileşenlerini ekle
        player.AddComponent<PlayerMovement>();
        player.AddComponent<PlayerHealth>();
        player.AddComponent<PlayerCombat>();
        player.AddComponent<PlayerAnimator>();
        player.AddComponent<PlayerDimensionController>();
        
        // Fizik bileşenlerini ekle
        player.AddComponent<BoxCollider2D>();
        var rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Sprite renderer ekle
        var spriteRenderer = player.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Player/player_idle.png");
        
        // Player'ı başlangıç pozisyonuna yerleştir
        player.transform.position = new Vector3(0, 0, 0);
    }

    private static void CreateEnvironment()
    {
        var environment = new GameObject("--- LEVEL ---");
        
        // Create ground
        var ground = new GameObject("Ground");
        ground.transform.SetParent(environment.transform);
        ground.AddComponent<BoxCollider2D>();
        var groundSprite = ground.AddComponent<SpriteRenderer>();
        groundSprite.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Environment/ground.png");
        ground.transform.position = new Vector3(0, -4, 0);
        ground.transform.localScale = new Vector3(20, 1, 1);
    }

    private static void SetupCamera()
    {
        var mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(0, 0, -10);
            mainCamera.backgroundColor = new Color(0.4f, 0.6f, 0.9f); // Light blue
        }
    }

    private static void SetupUI()
    {
        UISetup.SetupGameUI();
    }

    private static void EnsureDirectoryExists(string path)
    {
        if (!AssetDatabase.IsValidFolder(path))
        {
            string[] folders = path.Split('/');
            string currentPath = folders[0];
            for (int i = 1; i < folders.Length; i++)
            {
                string parentPath = currentPath;
                currentPath = $"{currentPath}/{folders[i]}";
                if (!AssetDatabase.IsValidFolder(currentPath))
                {
                    AssetDatabase.CreateFolder(parentPath, folders[i]);
                }
            }
        }
    }
} 