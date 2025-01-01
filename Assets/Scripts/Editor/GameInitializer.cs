using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GameInitializer
{
    [MenuItem("Tools/Setup/Initialize New Scene")]
    public static void InitializeScene()
    {
        // Save current scene first
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        // Setup basic scene structure
        CreateBasicStructure();

        // Setup UI
        TransitionSetup.SetupTransition();

        Debug.Log("Scene initialized successfully!");
    }

    private static void CreateBasicStructure()
    {
        // Create essential game objects
        CreateGameObject("--- MANAGERS ---");
        CreateGameObject("--- LEVEL ---");
        CreateGameObject("--- UI ---");
        CreateGameObject("--- AUDIO ---");

        // Create camera
        if (Camera.main == null)
        {
            var cam = new GameObject("Main Camera", typeof(Camera));
            cam.tag = "MainCamera";
            cam.transform.position = new Vector3(0, 0, -10);
        }
    }

    private static void CreateGameObject(string name)
    {
        var go = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
    }
} 