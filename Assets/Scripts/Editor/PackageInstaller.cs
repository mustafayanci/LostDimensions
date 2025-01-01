using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

public class PackageInstaller : EditorWindow
{
    private static AddRequest Request;
    private static string status = "";

    [MenuItem("Game/Package Installer")]
    public static void ShowWindow()
    {
        GetWindow<PackageInstaller>("Package Installer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Required Packages", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Install TextMeshPro"))
        {
            InstallTMP();
        }

        EditorGUILayout.HelpBox(status, MessageType.Info);
    }

    private static void InstallTMP()
    {
        status = "Installing TextMeshPro...";
        Request = Client.Add("com.unity.textmeshpro");
        EditorApplication.update += Progress;
    }

    private static void Progress()
    {
        if (Request.IsCompleted)
        {
            if (Request.Status == StatusCode.Success)
            {
                status = "TextMeshPro installed successfully!";
                TMPro.TMP_PackageUtilities.ImportTMPEssentials();
            }
            else if (Request.Status >= StatusCode.Failure)
            {
                status = $"Failed to install: {Request.Error.message}";
            }

            EditorApplication.update -= Progress;
        }
    }
} 