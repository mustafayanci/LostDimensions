using UnityEngine;
using UnityEditor;

public class TransitionSetup : EditorWindow
{
    [MenuItem("Tools/UI/Setup Transition")]
    public static void SetupTransition()
    {
        // Update deprecated method
        var canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No canvas found in scene!");
            return;
        }

        // Rest of your setup code...
    }
} 