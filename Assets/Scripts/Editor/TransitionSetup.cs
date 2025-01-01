using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TransitionSetup : Editor
{
    [MenuItem("Game/Setup Transition UI")]
    public static void SetupTransitionUI()
    {
        // Ana Canvas'ı bul veya oluştur
        Canvas mainCanvas = GameObject.FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            var canvasObj = new GameObject("GameCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            mainCanvas = canvasObj.GetComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            mainCanvas.sortingOrder = 100;
        }

        // Transition Panel
        var transitionObj = new GameObject("TransitionPanel", typeof(CanvasGroup), typeof(Image));
        transitionObj.transform.SetParent(mainCanvas.transform, false);
        
        var rectTransform = transitionObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        
        var image = transitionObj.GetComponent<Image>();
        image.color = Color.black;
        
        var canvasGroup = transitionObj.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        // TransitionManager'ı ekle
        var managerObj = new GameObject("TransitionManager", typeof(TransitionManager));
        var manager = managerObj.GetComponent<TransitionManager>();

        // Referansları ayarla
        var serializedObject = new SerializedObject(manager);
        var panelProperty = serializedObject.FindProperty("transitionPanel");
        var imageProperty = serializedObject.FindProperty("transitionImage");
        
        panelProperty.objectReferenceValue = canvasGroup;
        imageProperty.objectReferenceValue = image;
        
        serializedObject.ApplyModifiedProperties();

        Debug.Log("Transition UI setup completed!");
    }
} 