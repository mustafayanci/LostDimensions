using UnityEngine;
using UnityEditor;

public class GameSetup
{
    [MenuItem("Tools/Setup/Setup Game")]
    public static void SetupGame()
    {
        // Create Audio System
        AudioSetup.CreateAudioMixer();

        // Create UI
        TransitionSetup.SetupTransition();

        Debug.Log("Game setup completed!");
    }
} 