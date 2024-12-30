using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileControls : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Joystick moveJoystick;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button dashButton;
    
    [Header("Dimension Change")]
    [SerializeField] private Button dimensionChangeButton;
    
    private PlayerMovement playerMovement;
    private DimensionManager dimensionManager;
    private int currentDimension = 0;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        dimensionManager = DimensionManager.Instance;
        
        SetupButtons();
    }

    private void SetupButtons()
    {
        jumpButton.onClick.AddListener(() => playerMovement.Jump());
        dashButton.onClick.AddListener(() => playerMovement.StartCoroutine("Dash"));
        dimensionChangeButton.onClick.AddListener(ChangeDimension);
    }

    private void Update()
    {
        if (playerMovement != null)
        {
            // Joystick değerlerini oyuncu hareketine aktar
            float horizontalInput = moveJoystick.Horizontal;
            playerMovement.SetHorizontalInput(horizontalInput);
        }
    }

    private void ChangeDimension()
    {
        currentDimension = (currentDimension + 1) % 4; // 4 boyut varsayılıyor
        dimensionManager.ChangeDimension(currentDimension);
    }
} 