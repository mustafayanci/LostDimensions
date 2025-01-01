using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TransitionSetup : EditorWindow
{
    [MenuItem("Tools/UI/Setup Transition")]
    public static void SetupTransition()
    {
        // Find or create canvas
        var canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            var canvasObj = new GameObject("GameCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObj.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
        }

        // Create transition panel
        var transitionObj = new GameObject("TransitionPanel", typeof(CanvasGroup), typeof(Image));
        transitionObj.transform.SetParent(canvas.transform, false);
        
        var rectTransform = transitionObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        
        var image = transitionObj.GetComponent<Image>();
        image.color = Color.black;
        
        var canvasGroup = transitionObj.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        Debug.Log("Transition UI setup completed!");
    }
} 