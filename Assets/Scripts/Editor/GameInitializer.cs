using UnityEngine;
using UnityEditor;

public class GameInitializer : Editor
{
    [MenuItem("Game/Initialize Game")]
    public static void InitializeGame()
    {
        // 1. Proje yapısını oluştur
        ProjectSetup.SetupProjectStructure();
        
        // 2. UI sistemini kur
        UISetup.SetupUI();
        
        // 3. Transition sistemini kur
        TransitionSetup.SetupTransitionUI();
        
        // 4. GameCore'u oluştur
        CoreSetup.SetupGameCore();
        
        // 5. Sahneleri oluştur
        SceneSetup.SetupInitialScenes();
        
        Debug.Log("Game initialization completed!");
    }
} 