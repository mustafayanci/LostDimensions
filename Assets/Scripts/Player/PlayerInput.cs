using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector2> onMove = new UnityEvent<Vector2>();
    public UnityEvent onJumpPressed = new UnityEvent();
    public UnityEvent onJumpReleased = new UnityEvent();
    public UnityEvent onDashPressed = new UnityEvent();
    public UnityEvent onDimensionChangePressed = new UnityEvent();

    private void Update()
    {
        // Movement input
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        onMove?.Invoke(moveInput);

        // Jump input
        if (Input.GetButtonDown("Jump"))
            onJumpPressed?.Invoke();
        if (Input.GetButtonUp("Jump"))
            onJumpReleased?.Invoke();

        // Dash input
        if (Input.GetButtonDown("Dash"))
            onDashPressed?.Invoke();

        // Dimension change input
        if (Input.GetButtonDown("DimensionChange"))
            onDimensionChangePressed?.Invoke();
    }
} 