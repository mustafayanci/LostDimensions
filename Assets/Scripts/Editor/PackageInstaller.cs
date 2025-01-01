using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

public class PackageInstaller
{
    [MenuItem("Tools/Setup/Install Required Packages")]
    public static void InstallRequiredPackages()
    {
        // Install TextMeshPro
        InstallPackage("com.unity.textmeshpro");
        
        // Install other required packages
        InstallPackage("com.unity.2d.sprite");
        InstallPackage("com.unity.2d.animation");
    }

    private static void InstallPackage(string packageId)
    {
        var request = Client.Add(packageId);
        
        EditorApplication.update += () =>
        {
            if (request.Status == StatusCode.Failure)
            {
                Debug.LogError($"Failed to install package: {packageId}");
            }
            else if (request.Status == StatusCode.Success)
            {
                Debug.Log($"Successfully installed package: {packageId}");
                
                // If it's TextMeshPro, import essentials
                if (packageId == "com.unity.textmeshpro")
                {
                    ImportTMPEssentials();
                }
            }
        };
    }

    private static void ImportTMPEssentials()
    {
        // Use AssetDatabase to import TMP essentials
        AssetDatabase.ImportPackage("Packages/com.unity.textmeshpro/Package Resources/TMP Essential Resources.unitypackage", false);
    }
} 