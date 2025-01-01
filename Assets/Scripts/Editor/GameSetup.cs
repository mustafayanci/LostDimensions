using UnityEngine;
using UnityEditor;

public class GameSetup : Editor
{
    [MenuItem("Game/Setup Complete Game")]
    public static void SetupCompleteGame()
    {
        // 1. Proje yapısını oluştur
        ProjectSetup.SetupProjectStructure();

        // 2. Audio sistemini kur
        AudioSetup.SetupAudioSystem();

        // 3. UI sistemini kur
        UISetup.SetupUI();

        // 4. GameCore'u oluştur
        CoreSetup.SetupGameCore();

        // 5. Temel sahneleri oluştur
        SceneSetup.SetupInitialScenes();

        Debug.Log("Complete game setup finished!");
    }
} 