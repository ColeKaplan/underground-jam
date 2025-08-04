using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MoleMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 200; // Degrees per second for smooth turning

    private InputAction move;
    private Rigidbody2D rb;
    private Vector2 inputDirection = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        move = InputSystem.actions.FindAction("MoleMove");
    }

    private void OnEnable()
    {
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void Update()
    {
        inputDirection = move.ReadValue<Vector2>();

        // Rotate mole based on movement direction
        if (inputDirection != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;

            if (rb.linearVelocity.magnitude < 0.1f)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
                return;
            }
            float currentAngle = transform.eulerAngles.z;

            // Smooth rotation

            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = inputDirection * moveSpeed;
    }
}
