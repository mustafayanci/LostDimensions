using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using UnityEditor.Audio;

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

        // Create mixer through the audio mixer template
        AudioMixerController mixer = AudioMixerController.CreateDefaultAudioMixer();
        AssetDatabase.CreateAsset(mixer, path);

        // Create groups
        var masterGroup = mixer.outputAudioMixerGroup;
        var musicGroup = mixer.AddGroup("Music");
        var sfxGroup = mixer.AddGroup("SFX");

        // Add volume parameters
        mixer.SetFloat("MusicVolume", 0f);
        mixer.SetFloat("SFXVolume", 0f);

        // Save the changes
        EditorUtility.SetDirty(mixer);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Audio Mixer created successfully!");
    }
} 