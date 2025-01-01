using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Interfaces;

public class MobileControls : MonoBehaviour
{
    [SerializeField] private Button jumpButton;
    [SerializeField] private Joystick movementJoystick;

    private IPlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindFirstObjectByType<MonoBehaviour>() as IPlayerMovement;
        
        if (jumpButton != null)
        {
            jumpButton.onClick.AddListener(OnJumpButtonPressed);
        }
    }

    private void OnJumpButtonPressed()
    {
        if (playerMovement != null)
        {
            playerMovement.Jump();
        }
    }

    private void Update()
    {
        if (playerMovement != null && movementJoystick != null)
        {
            playerMovement.SetHorizontalInput(movementJoystick.Horizontal);
        }
    }
} 