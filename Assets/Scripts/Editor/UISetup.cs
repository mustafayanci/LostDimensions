using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class UISetup : Editor
{
    [MenuItem("Game/Setup UI System")]
    public static void SetupUI()
    {
        // Ana Canvas'ı oluştur
        var canvas = CreateMainCanvas();
        
        // HUD elementlerini oluştur
        CreateHUD(canvas);
        
        // Menü panellerini oluştur
        CreateMenuPanels(canvas);
        
        // Geçiş efektlerini oluştur
        CreateTransitionEffects(canvas);

        Debug.Log("UI system setup completed!");
    }

    private static GameObject CreateMainCanvas()
    {
        var canvasObj = new GameObject("UICanvas");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        return canvasObj;
    }

    private static void CreateHUD(GameObject canvas)
    {
        var hudPanel = CreatePanel(canvas, "HUDPanel");
        
        // Health Bar
        var healthBar = CreateHealthBar(hudPanel);
        
        // Dimension Display
        var dimensionText = CreateDimensionDisplay(hudPanel);
        
        // Score Display
        var scoreText = CreateScoreDisplay(hudPanel);
    }

    private static void CreateMenuPanels(GameObject canvas)
    {
        // Pause Menu
        var pausePanel = CreatePanel(canvas, "PausePanel");
        pausePanel.SetActive(false);
        CreateMenuButtons(pausePanel, new string[] { "Resume", "Restart", "Quit" });
        
        // Game Over Menu
        var gameOverPanel = CreatePanel(canvas, "GameOverPanel");
        gameOverPanel.SetActive(false);
        CreateMenuButtons(gameOverPanel, new string[] { "Retry", "Menu" });
    }

    private static void CreateTransitionEffects(GameObject canvas)
    {
        var transitionPanel = CreatePanel(canvas, "TransitionPanel");
        var canvasGroup = transitionPanel.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        
        var image = transitionPanel.GetComponent<Image>();
        image.color = Color.black;
    }

    private static GameObject CreatePanel(GameObject parent, string name)
    {
        var panel = new GameObject(name);
        panel.transform.SetParent(parent.transform, false);
        
        var rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        
        panel.AddComponent<Image>();
        
        return panel;
    }

    private static void CreateMenuButtons(GameObject panel, string[] buttonNames)
    {
        var buttonContainer = CreatePanel(panel, "ButtonContainer");
        var containerRect = buttonContainer.GetComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(200, buttonNames.Length * 60);
        
        for (int i = 0; i < buttonNames.Length; i++)
        {
            CreateButton(buttonContainer, buttonNames[i], new Vector2(0, -i * 60));
        }
    }

    private static GameObject CreateButton(GameObject parent, string text, Vector2 position)
    {
        var button = new GameObject(text + "Button");
        button.transform.SetParent(parent.transform, false);
        
        var rect = button.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(160, 40);
        rect.anchoredPosition = position;
        
        button.AddComponent<Image>();
        button.AddComponent<Button>();
        
        var textObj = new GameObject("Text");
        textObj.transform.SetParent(button.transform, false);
        
        var textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        var tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.alignment = TextAlignmentOptions.Center;
        
        return button;
    }

    private static GameObject CreateHealthBar(GameObject parent)
    {
        var healthBarObj = CreatePanel(parent, "HealthBar");
        var rect = healthBarObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(20, -20);
        rect.sizeDelta = new Vector2(200, 20);

        var slider = healthBarObj.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 1;

        var background = CreatePanel(healthBarObj, "Background");
        var fill = CreatePanel(healthBarObj, "Fill");
        
        slider.targetGraphic = background.GetComponent<Image>();
        slider.fillRect = fill.GetComponent<RectTransform>();
        
        return healthBarObj;
    }

    private static GameObject CreateDimensionDisplay(GameObject parent)
    {
        var dimensionObj = new GameObject("DimensionDisplay");
        dimensionObj.transform.SetParent(parent.transform, false);
        
        var rect = dimensionObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.anchoredPosition = new Vector2(-20, -20);
        rect.sizeDelta = new Vector2(200, 30);

        var tmp = dimensionObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "Dimension: 0";
        tmp.alignment = TextAlignmentOptions.Right;
        tmp.fontSize = 24;
        
        return dimensionObj;
    }

    private static GameObject CreateScoreDisplay(GameObject parent)
    {
        var scoreObj = new GameObject("ScoreDisplay");
        scoreObj.transform.SetParent(parent.transform, false);
        
        var rect = scoreObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1);
        rect.anchorMax = new Vector2(0.5f, 1);
        rect.anchoredPosition = new Vector2(0, -20);
        rect.sizeDelta = new Vector2(200, 30);

        var tmp = scoreObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "Score: 0";
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 24;
        
        return scoreObj;
    }
} 