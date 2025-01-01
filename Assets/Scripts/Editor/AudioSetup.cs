using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;

public class AudioSetup : Editor
{
    [MenuItem("Game/Setup Audio System")]
    public static void SetupAudioSystem()
    {
        // Ana Audio Mixer'ı oluştur
        var mixer = AssetDatabase.LoadAssetAtPath<AudioMixer>("Assets/Audio/GameAudioMixer.mixer");
        if (mixer == null)
        {
            mixer = new AudioMixer();
            AssetDatabase.CreateAsset(mixer, "Assets/Audio/GameAudioMixer.mixer");

            // Mixer gruplarını oluştur
            var masterGroup = mixer.FindMatchingGroups("Master")[0];
            var musicGroup = mixer.CreateGroup("Music");
            var sfxGroup = mixer.CreateGroup("SFX");

            // Exposed parametreleri ekle
            mixer.SetFloat("MusicVolume", 0f);
            mixer.SetFloat("SFXVolume", 0f);
        }

        // Temel ses efektlerini ayarla
        SetupBasicSounds();

        Debug.Log("Audio system setup completed!");
    }

    private static void SetupBasicSounds()
    {
        var sounds = new string[]
        {
            "PlayerJump",
            "PlayerHurt",
            "PlayerDeath",
            "EnemyHit",
            "EnemyDeath",
            "CheckpointActivated",
            "DimensionChange_0",
            "DimensionChange_1",
            "DimensionChange_2",
            "DimensionChange_3",
            "LevelComplete",
            "GameOver",
            "ButtonClick"
        };

        // Her ses için bir placeholder AudioClip oluştur
        foreach (var sound in sounds)
        {
            var path = $"Assets/Audio/SFX/{sound}.wav";
            if (!AssetDatabase.LoadAssetAtPath<AudioClip>(path))
            {
                var clip = AudioClip.Create(sound, 44100, 1, 44100, false);
                AssetDatabase.CreateAsset(clip, path);
            }
        }
    }
} 