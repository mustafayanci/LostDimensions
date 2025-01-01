using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;

public class AudioSetup
{
    [MenuItem("Tools/Audio/Create Audio Mixer")]
    public static void CreateAudioMixer()
    {
        // Create the mixer asset
        string path = "Assets/Audio/MainMixer.mixer";
        
        // Check if mixer already exists
        var existingMixer = AssetDatabase.LoadAssetAtPath<AudioMixer>(path);
        if (existingMixer != null)
        {
            Debug.LogWarning("Audio Mixer already exists!");
            return;
        }

        // Ensure the Audio directory exists
        if (!AssetDatabase.IsValidFolder("Assets/Audio"))
        {
            AssetDatabase.CreateFolder("Assets", "Audio");
        }

        // Create a new Audio Mixer
        var mixer = AssetDatabase.LoadAssetAtPath<AudioMixer>("Packages/com.unity.audio.default-mixer/Runtime/DefaultAudioMixer.mixer");
        if (mixer == null)
        {
            Debug.LogError("Could not find default audio mixer template!");
            return;
        }

        // Create a copy of the default mixer
        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(mixer), path);
        var newMixer = AssetDatabase.LoadAssetAtPath<AudioMixer>(path);

        // Get the mixer groups
        var groups = newMixer.FindMatchingGroups(string.Empty);
        var masterGroup = groups[0];

        // Create new groups if they don't exist
        bool hasMusicGroup = false;
        bool hasSFXGroup = false;

        foreach (var group in groups)
        {
            if (group.name == "Music") hasMusicGroup = true;
            if (group.name == "SFX") hasSFXGroup = true;
        }

        // Add exposed parameters
        newMixer.SetFloat("MusicVolume", 0f);
        newMixer.SetFloat("SFXVolume", 0f);

        // Save the changes
        EditorUtility.SetDirty(newMixer);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Audio Mixer created successfully!");
    }
} 