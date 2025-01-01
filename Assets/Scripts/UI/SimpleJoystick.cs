using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField] private float deadZone = 0f;
    [SerializeField] private float moveRange = 50f;

    private Vector2 input = Vector2.zero;
    private Vector2 startPos;
    private bool isDragging = false;

    public float Horizontal => input.x;
    public float Vertical => input.y;

    private void Awake()
    {
        startPos = background.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 position = eventData.position - startPos;
        position = Vector2.ClampMagnitude(position, moveRange);
        handle.anchoredPosition = position;

        float magnitude = position.magnitude;
        if (magnitude > deadZone)
        {
            input = position.normalized * ((magnitude - deadZone) / (moveRange - deadZone));
        }
        else
        {
            input = Vector2.zero;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
} 