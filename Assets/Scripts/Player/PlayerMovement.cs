using UnityEngine;
using Interfaces;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float wallJumpForce = 8f;
    [SerializeField] private float dashForce = 15f;
    
    [Header("Ground & Wall Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius = 0.2f;
    
    [Header("Dash Settings")]
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isWallSliding;
    private bool canDash = true;
    private float horizontalInput;
    private bool facingRight = true;
    private bool isDashing;
    private bool jumpInput = false;
    private Vector2 moveInput = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDashing) return;

        CheckEnvironment();
        HandleInput();
        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        
        Move();
        
        if (jumpInput && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AudioManager.Instance.PlaySound("PlayerJump");
            jumpInput = false;
        }
    }

    private void CheckEnvironment()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        isWallSliding = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded) Jump();
            else if (isWallSliding) WallJump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void Move()
    {
        float targetVelocityX = horizontalInput * moveSpeed;
        rb.linearVelocity = new Vector2(targetVelocityX, rb.linearVelocity.y);
    }

    public void Jump()
    {
        jumpInput = true;
    }

    private void WallJump()
    {
        float wallJumpDirection = -Mathf.Sign(horizontalInput);
        rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpForce, jumpForce);
    }

    private System.Collections.IEnumerator Dash()
    {
        AudioManager.Instance.PlaySound("PlayerDash");
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        
        rb.linearVelocity = new Vector2(transform.localScale.x * dashForce, 0);
        
        yield return new WaitForSeconds(dashDuration);
        
        isDashing = false;
        rb.gravityScale = originalGravity;
        
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void Flip()
    {
        if (horizontalInput > 0 && !facingRight || horizontalInput < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public void SetHorizontalInput(float input)
    {
        moveInput.x = input;
    }
} 