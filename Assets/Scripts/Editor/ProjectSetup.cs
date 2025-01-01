using UnityEngine;
using UnityEditor;
using System.IO;

public class ProjectSetup
{
    [MenuItem("Tools/Setup/Create Folder Structure")]
    public static void CreateFolderStructure()
    {
        CreateFolders(new string[]
        {
            "Assets/Art",
            "Assets/Art/Player",
            "Assets/Art/Environment",
            "Assets/Art/UI",
            "Assets/Audio",
            "Assets/Audio/Music",
            "Assets/Audio/SFX",
            "Assets/Prefabs",
            "Assets/Scenes",
            "Assets/Materials"
        });

        CreateDefaultAssets();
        AssetDatabase.Refresh();
    }

    private static void CreateFolders(string[] folders)
    {
        foreach (var folder in folders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                string parentPath = Path.GetDirectoryName(folder);
                string newFolderName = Path.GetFileName(folder);
                AssetDatabase.CreateFolder(parentPath, newFolderName);
            }
        }
    }

    private static void CreateDefaultAssets()
    {
        // Create default sprites
        CreateDefaultSprite("Assets/Art/Player/player_idle.png");
        CreateDefaultSprite("Assets/Art/Environment/ground.png");
    }

    private static void CreateDefaultSprite(string path)
    {
        if (!File.Exists(path))
        {
            // Create a simple white texture
            var tex = new Texture2D(32, 32);
            var colors = new Color[32 * 32];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = Color.white;
            tex.SetPixels(colors);
            tex.Apply();

            // Save as PNG
            byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
            
            // Set texture import settings
            AssetDatabase.ImportAsset(path);
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spritePixelsPerUnit = 32;
                importer.filterMode = FilterMode.Point;
                importer.SaveAndReimport();
            }
        }
    }
} 