using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneSetup : Editor
{
    [MenuItem("Game/Setup Initial Scenes")]
    public static void SetupInitialScenes()
    {
        // MainMenu sahnesini oluştur
        var mainMenuScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        var gameCore = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Core/GameCore.prefab"));
        EditorSceneManager.SaveScene(mainMenuScene, "Assets/Scenes/MainMenu.unity");

        // Level1 sahnesini oluştur
        var level1Scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        CreateBasicLevel();
        EditorSceneManager.SaveScene(level1Scene, "Assets/Scenes/Level1.unity");

        // Build settings'e sahneleri ekle
        EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
        {
            new EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true),
            new EditorBuildSettingsScene("Assets/Scenes/Level1.unity", true)
        };

        Debug.Log("Initial scenes created successfully!");
    }

    private static void CreateBasicLevel()
    {
        // Temel platform oluştur
        var platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
        platform.transform.localScale = new Vector3(20f, 1f, 1f);
        platform.transform.position = new Vector3(0f, -2f, 0f);
        platform.name = "Ground";

        // Player spawn noktası oluştur
        var spawnPoint = new GameObject("PlayerSpawnPoint");
        spawnPoint.transform.position = new Vector3(0f, 0f, 0f);
    }
} 