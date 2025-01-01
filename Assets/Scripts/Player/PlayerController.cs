using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing;
    private float dashTimeLeft;
    private Vector2 moveInput;
    private bool jumpInput;
    private bool dashInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) jumpInput = true;
        if (Input.GetButtonDown("Dash")) dashInput = true;

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            HandleDash();
            return;
        }

        // Movement
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        // Jump
        if (jumpInput && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AudioManager.Instance.PlaySound("PlayerJump");
        }

        // Dash
        if (dashInput && canDash)
        {
            StartDash();
        }

        // Reset inputs
        jumpInput = false;
        dashInput = false;
    }

    private void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
        rb.gravityScale = 0;
        AudioManager.Instance.PlaySound("PlayerDash");
    }

    private void HandleDash()
    {
        if (dashTimeLeft > 0)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
            dashTimeLeft -= Time.fixedDeltaTime;
        }
        else
        {
            isDashing = false;
            rb.gravityScale = 3;
            if (isGrounded) canDash = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canDash = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
} 