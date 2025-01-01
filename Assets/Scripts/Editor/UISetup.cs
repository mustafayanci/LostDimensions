using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class UISetup
{
    [MenuItem("Tools/UI/Setup Game UI")]
    public static void SetupGameUI()
    {
        // Ana Canvas'ı oluştur
        var canvas = CreateMainCanvas();
        
        // UI Manager'ı oluştur
        var uiManager = CreateUIManager();

        // HUD Panel
        var hudPanel = CreateHUDPanel(canvas.transform);
        CreateHealthBar(hudPanel.transform);
        CreateDimensionDisplay(hudPanel.transform);

        // Pause Panel
        var pausePanel = CreatePausePanel(canvas.transform);
        CreatePauseMenu(pausePanel.transform);

        // Game Over Panel
        var gameOverPanel = CreateGameOverPanel(canvas.transform);
        CreateGameOverMenu(gameOverPanel.transform);

        // Transition Panel
        var transitionPanel = CreateTransitionPanel(canvas.transform);

        // UI Manager referanslarını ayarla
        SetupUIManagerReferences(uiManager, hudPanel, pausePanel, gameOverPanel, transitionPanel);

        Debug.Log("Game UI setup completed!");
    }

    private static Canvas CreateMainCanvas()
    {
        var canvasObj = new GameObject("GameCanvas");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        return canvas;
    }

    private static UIManager CreateUIManager()
    {
        var uiManagerObj = new GameObject("UIManager");
        return uiManagerObj.AddComponent<UIManager>();
    }

    private static GameObject CreateHUDPanel(Transform parent)
    {
        var panel = CreatePanel("HUDPanel", parent);
        panel.GetComponent<CanvasGroup>().alpha = 1;
        return panel;
    }

    private static void CreateHealthBar(Transform parent)
    {
        var healthBar = new GameObject("HealthBar", typeof(Slider));
        healthBar.transform.SetParent(parent, false);
        healthBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350, 200);
        
        var slider = healthBar.GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 1;
    }

    private static void CreateDimensionDisplay(Transform parent)
    {
        var dimensionText = new GameObject("DimensionText", typeof(TextMeshProUGUI));
        dimensionText.transform.SetParent(parent, false);
        dimensionText.GetComponent<RectTransform>().anchoredPosition = new Vector2(350, 200);
        
        var tmp = dimensionText.GetComponent<TextMeshProUGUI>();
        tmp.text = "Normal";
        tmp.fontSize = 36;
        tmp.alignment = TextAlignmentOptions.Center;
    }

    private static GameObject CreatePausePanel(Transform parent)
    {
        var panel = CreatePanel("PausePanel", parent);
        panel.SetActive(false);
        return panel;
    }

    private static void CreatePauseMenu(Transform parent)
    {
        var title = CreateText(parent, "PAUSED", new Vector2(0, 100));
        CreateButton(parent, "Resume", new Vector2(0, 0), "Resume");
        CreateButton(parent, "Main Menu", new Vector2(0, -100), "MainMenu");
    }

    private static GameObject CreateGameOverPanel(Transform parent)
    {
        var panel = CreatePanel("GameOverPanel", parent);
        panel.SetActive(false);
        return panel;
    }

    private static void CreateGameOverMenu(Transform parent)
    {
        var title = CreateText(parent, "GAME OVER", new Vector2(0, 100));
        CreateButton(parent, "Retry", new Vector2(0, 0), "Retry");
        CreateButton(parent, "Main Menu", new Vector2(0, -100), "MainMenu");
    }

    private static GameObject CreateTransitionPanel(Transform parent)
    {
        var panel = CreatePanel("TransitionPanel", parent);
        var image = panel.GetComponent<Image>();
        image.color = Color.black;
        
        var canvasGroup = panel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        
        return panel;
    }

    private static GameObject CreatePanel(string name, Transform parent)
    {
        var panel = new GameObject(name, typeof(CanvasGroup), typeof(Image));
        panel.transform.SetParent(parent, false);
        
        var rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        
        var image = panel.GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0.8f);
        
        return panel;
    }

    private static TextMeshProUGUI CreateText(Transform parent, string text, Vector2 position)
    {
        var textObj = new GameObject(text + "Text", typeof(TextMeshProUGUI));
        textObj.transform.SetParent(parent, false);
        
        var rectTransform = textObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        
        var tmp = textObj.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 48;
        tmp.alignment = TextAlignmentOptions.Center;
        
        return tmp;
    }

    private static Button CreateButton(Transform parent, string text, Vector2 position, string buttonName)
    {
        var buttonObj = new GameObject(buttonName + "Button", typeof(Image), typeof(Button));
        buttonObj.transform.SetParent(parent, false);
        
        var rectTransform = buttonObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(200, 50);
        
        var buttonText = CreateText(buttonObj.transform, text, Vector2.zero);
        buttonText.fontSize = 24;
        
        return buttonObj.GetComponent<Button>();
    }

    private static void SetupUIManagerReferences(UIManager uiManager, GameObject hudPanel, GameObject pausePanel, 
        GameObject gameOverPanel, GameObject transitionPanel)
    {
        var serializedObject = new SerializedObject(uiManager);
        
        SetSerializedProperty(serializedObject, "hudPanel", hudPanel);
        SetSerializedProperty(serializedObject, "pausePanel", pausePanel);
        SetSerializedProperty(serializedObject, "gameOverPanel", gameOverPanel);
        SetSerializedProperty(serializedObject, "transitionPanel", transitionPanel.GetComponent<CanvasGroup>());
        
        serializedObject.ApplyModifiedProperties();
    }

    private static void SetSerializedProperty(SerializedObject serializedObject, string propertyName, Object value)
    {
        var property = serializedObject.FindProperty(propertyName);
        if (property != null)
        {
            property.objectReferenceValue = value;
        }
    }
} 