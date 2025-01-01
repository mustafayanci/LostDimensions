using UnityEngine;
using UnityEditor;

public class CoreSetup : Editor
{
    [MenuItem("Game/Setup Game Core")]
    public static void SetupGameCore()
    {
        // GameCore GameObject'i oluştur
        var gameCore = new GameObject("GameCore");
        
        // Yönetici sınıfları ekle
        gameCore.AddComponent<GameManager>();
        gameCore.AddComponent<LevelManager>();
        gameCore.AddComponent<DimensionManager>();
        gameCore.AddComponent<TransitionManager>();
        gameCore.AddComponent<AudioManager>();

        // Prefab olarak kaydet
        string prefabPath = "Assets/Prefabs/Core/GameCore.prefab";
        PrefabUtility.SaveAsPrefabAsset(gameCore, prefabPath);
        
        // Sahne objesini sil
        DestroyImmediate(gameCore);

        Debug.Log("GameCore prefab created successfully!");
    }
} 