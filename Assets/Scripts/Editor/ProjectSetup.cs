using UnityEngine;
using UnityEditor;
using System.IO;

public class ProjectSetup : Editor
{
    [MenuItem("Game/Setup Project Structure")]
    public static void SetupProjectStructure()
    {
        string[] folders = new string[]
        {
            "Animations",
            "Audio/Music",
            "Audio/SFX",
            "Materials",
            "Prefabs/Core",
            "Prefabs/Player",
            "Prefabs/Enemies",
            "Prefabs/Puzzles",
            "Prefabs/UI",
            "Scenes",
            "Scripts/Core",
            "Scripts/Editor",
            "Scripts/Enemy",
            "Scripts/Level",
            "Scripts/Player",
            "Scripts/Puzzle",
            "Scripts/UI",
            "Sprites"
        };

        foreach (var folder in folders)
        {
            string fullPath = Path.Combine(Application.dataPath, folder);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }
        
        AssetDatabase.Refresh();
        Debug.Log("Project structure created successfully!");
    }
} 