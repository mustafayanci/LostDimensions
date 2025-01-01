using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;

public class AudioSetup
{
    [MenuItem("Tools/Audio/Create Audio Mixer")]
    public static void CreateAudioMixer()
    {
        // Create the mixer asset
        var mixer = ScriptableObject.CreateInstance<AudioMixer>();
        
        // Create the asset file
        string path = "Assets/Audio/MainMixer.mixer";
        AssetDatabase.CreateAsset(mixer, path);

        // Create groups
        var masterGroup = mixer.outputAudioMixerGroup;
        var musicGroup = AssetDatabase.LoadAssetAtPath<AudioMixerGroup>("Assets/Audio/Music.mixer");
        var sfxGroup = AssetDatabase.LoadAssetAtPath<AudioMixerGroup>("Assets/Audio/SFX.mixer");

        if (musicGroup == null || sfxGroup == null)
        {
            Debug.LogError("Failed to create audio mixer groups!");
            return;
        }

        // Save the changes
        EditorUtility.SetDirty(mixer);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Audio Mixer created successfully!");
    }
} 