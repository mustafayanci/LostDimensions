using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class UISetup : Editor
{
    [MenuItem("Game/Setup UI")]
    public static void SetupUI()
    {
        // EventSystem kontrolü
        if (GameObject.FindObjectOfType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        // Mevcut Canvas'ı kontrol et
        var existingCanvas = GameObject.Find("GameCanvas");
        if (existingCanvas != null)
        {
            DestroyImmediate(existingCanvas);
        }

        // Canvas oluştur
        var canvasObj = new GameObject("GameCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasObj.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1;

        // CanvasScaler ayarları
        var scaler = canvasObj.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        // Joystick Panel
        var joystickPanel = CreateUIElement("JoystickPanel", canvasObj, typeof(CanvasRenderer), typeof(Image));
        var joystickRT = joystickPanel.GetComponent<RectTransform>();
        joystickRT.anchorMin = new Vector2(0, 0);
        joystickRT.anchorMax = new Vector2(0, 0);
        joystickRT.pivot = new Vector2(0.5f, 0.5f);
        joystickRT.anchoredPosition = new Vector2(150, 150);
        joystickRT.sizeDelta = new Vector2(200, 200);
        var panelImage = joystickPanel.GetComponent<Image>();
        panelImage.color = new Color(1, 1, 1, 0); // Tamamen saydam

        // Joystick Background
        var background = CreateUIElement("JoystickBackground", joystickPanel, typeof(Image));
        var bgRT = background.GetComponent<RectTransform>();
        bgRT.sizeDelta = new Vector2(150, 150);
        var bgImage = background.GetComponent<Image>();
        bgImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        bgImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        bgImage.type = Image.Type.Sliced;
        bgImage.raycastTarget = true;

        // Joystick Handle
        var handle = CreateUIElement("JoystickHandle", background, typeof(Image));
        var handleRT = handle.GetComponent<RectTransform>();
        handleRT.sizeDelta = new Vector2(75, 75);
        var handleImage = handle.GetComponent<Image>();
        handleImage.color = Color.white;
        handleImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        handleImage.type = Image.Type.Sliced;
        handleImage.raycastTarget = true;

        // Butonlar
        CreateButton("JumpButton", canvasObj, new Vector2(1, 0), new Vector2(-200, 100), new Vector2(120, 120), "JUMP");
        CreateButton("DashButton", canvasObj, new Vector2(1, 0), new Vector2(-350, 100), new Vector2(120, 120), "DASH");
        CreateButton("DimensionButton", canvasObj, new Vector2(1, 1), new Vector2(-100, -50), new Vector2(160, 60), "DIMENSION");

        // Script'leri ekle
        var joystickScript = joystickPanel.AddComponent<SimpleJoystick>();
        joystickScript.background = background.GetComponent<RectTransform>();
        joystickScript.handle = handle.GetComponent<RectTransform>();

        var mobileControls = canvasObj.AddComponent<MobileControls>();
        mobileControls.moveJoystick = joystickScript;
        mobileControls.jumpButton = GameObject.Find("JumpButton").GetComponent<Button>();
        mobileControls.dashButton = GameObject.Find("DashButton").GetComponent<Button>();
        mobileControls.dimensionChangeButton = GameObject.Find("DimensionButton").GetComponent<Button>();

        Debug.Log("UI setup completed successfully!");
        
        // Scene'i kaydet
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }

    private static GameObject CreateUIElement(string name, GameObject parent, params System.Type[] components)
    {
        var go = new GameObject(name, components);
        go.transform.SetParent(parent.transform, false);
        return go;
    }

    private static void CreateButton(string name, GameObject parent, Vector2 anchor, Vector2 position, Vector2 size, string text)
    {
        var button = CreateUIElement(name, parent, typeof(CanvasRenderer), typeof(Image), typeof(Button));
        var rt = button.GetComponent<RectTransform>();
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;

        // Buton görünümünü ayarla
        var buttonImage = button.GetComponent<Image>();
        buttonImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        buttonImage.type = Image.Type.Sliced;
        buttonImage.color = new Color(0.8f, 0.8f, 0.8f, 0.9f);

        var textObj = CreateUIElement(name + "Text", button, typeof(CanvasRenderer), typeof(Text));
        var textComp = textObj.GetComponent<Text>();
        textComp.text = text;
        textComp.alignment = TextAnchor.MiddleCenter;
        textComp.color = Color.black;
        textComp.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textComp.fontSize = 24;

        // Text RectTransform ayarları
        var textRT = textObj.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.sizeDelta = Vector2.zero;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;
    }
} 