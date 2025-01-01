using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

public class PackageInstaller
{
    private static AddRequest Request;

    [MenuItem("Game/Install Required Packages")]
    public static void InstallRequiredPackages()
    {
        // TextMeshPro paketini ekle
        Request = Client.Add("com.unity.textmeshpro");
        EditorApplication.update += Progress;
    }

    private static void Progress()
    {
        if (Request.IsCompleted)
        {
            if (Request.Status == StatusCode.Success)
                Debug.Log("Installed: " + Request.Result.packageId);
            else if (Request.Status >= StatusCode.Failure)
                Debug.Log(Request.Error.message);

            EditorApplication.update -= Progress;
        }
    }
} 