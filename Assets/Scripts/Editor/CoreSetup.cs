using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

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

    private static void SetupAudioManager(GameObject gameCore)
    {
        var audioManager = gameCore.AddComponent<AudioManager>();
        var mixer = AssetDatabase.LoadAssetAtPath<AudioMixer>("Assets/Audio/GameAudioMixer.mixer");
        
        var serializedObject = new SerializedObject(audioManager);
        var mixerProperty = serializedObject.FindProperty("audioMixer");
        mixerProperty.objectReferenceValue = mixer;

        // Temel sesleri yükle
        var soundsProperty = serializedObject.FindProperty("sounds");
        soundsProperty.arraySize = 13; // Temel ses sayısı

        for (int i = 0; i < soundsProperty.arraySize; i++)
        {
            var soundElement = soundsProperty.GetArrayElementAtIndex(i);
            var nameProperty = soundElement.FindPropertyRelative("name");
            var clipProperty = soundElement.FindPropertyRelative("clip");
            var mixerGroupProperty = soundElement.FindPropertyRelative("mixerGroup");

            // Ses ayarlarını yap
            // ...
        }

        serializedObject.ApplyModifiedProperties();
    }
} 