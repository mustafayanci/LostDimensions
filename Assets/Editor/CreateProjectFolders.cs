using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateProjectFolders : EditorWindow
{
    [MenuItem("Tools/Create Project Folders")]
    static void CreateFolders()
    {
        string[] folders = new string[]
        {
            "Scripts",
            "Scripts/Editor",
            "Scenes",
            "Prefabs",
            "Materials",
            "Models",
            "Models/Characters",
            "Models/Environment",
            "Textures",
            "Audio",
            "Audio/SFX",
            "Audio/Music",
            "Animations",
            "UI",
            "UI/Sprites",
            "Resources",
            "Shaders",
            "ScriptableObjects",
            "Settings",
            "Plugins",
            "StreamingAssets",
            "UI/Fonts",
            "UI/Icons",
            "VFX"
        };

        foreach (string folder in folders)
        {
            string folderPath = Path.Combine(Application.dataPath, folder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
        
        AssetDatabase.Refresh();
        Debug.Log("Proje klasörleri başarıyla oluşturuldu!");
    }
} 