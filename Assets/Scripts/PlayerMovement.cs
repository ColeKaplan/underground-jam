using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Jump Feel Settings")]
    [SerializeField] private float coyoteTimeDuration = 0.1f;
    [SerializeField] private float jumpBufferDuration = 0.1f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float fallGravityMultiplier = 2.5f;
    [SerializeField] private float lowJumpGravityMultiplier = 2f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem jumpParticles;

    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.2f;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction jumpAction;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveInput;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float originalGravity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalGravity = rb.gravityScale;
        moveAction = InputSystem.actions.FindAction("PlayerMove");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTimeDuration;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpAction.WasPressedThisFrame())
        {
            jumpBufferCounter = jumpBufferDuration;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            if (jumpParticles != null)
            {
                jumpParticles.Play();
            }
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpBufferCounter = 0f;
        }

        if (jumpAction.WasReleasedThisFrame() && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            coyoteTimeCounter = 0f;
        }
    }

    private void FixedUpdate()
    {
        CheckGroundStatus();
        ApplyMovement();
        ApplyVariableGravity();
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        if (moveInput.x > 0f)
        {
            spriteRenderer.flipX = false;
        } else if(moveInput.x < 0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void ApplyVariableGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = originalGravity * fallGravityMultiplier;
        }
        else if (rb.linearVelocity.y > 0 && !jumpAction.IsPressed())
        {
            rb.gravityScale = originalGravity * lowJumpGravityMultiplier;
        }
        else
        {
            rb.gravityScale = originalGravity;
        }
    }
}
