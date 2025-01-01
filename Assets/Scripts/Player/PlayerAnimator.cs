using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerController controller;
    private Rigidbody2D rb;

    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int VerticalVelocity = Animator.StringToHash("verticalVelocity");
    private static readonly int IsDashing = Animator.StringToHash("isDashing");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Hareket yönüne göre sprite'ı çevir
        if (rb.velocity.x != 0)
        {
            spriteRenderer.flipX = rb.velocity.x < 0;
        }

        // Animator parametrelerini güncelle
        animator.SetBool(IsGrounded, controller.IsGrounded);
        animator.SetBool(IsRunning, Mathf.Abs(rb.velocity.x) > 0.1f);
        animator.SetFloat(VerticalVelocity, rb.velocity.y);
        animator.SetBool(IsDashing, controller.IsDashing);
    }

    public void PlayHurtAnimation()
    {
        animator.SetTrigger("Hurt");
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Death");
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }
} 