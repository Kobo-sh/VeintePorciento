using UnityEngine;  // Librería principal de Unity.
public class PlayerMovementModel : MonoBehaviour
{
    [Header("Referencias")]
    // Referencia al script que lee el input.
    [SerializeField] private PlayerInputController playerInputController;
    // Referencia al Rigidbody del personaje.
    [SerializeField] private Rigidbody rb;
    // Referencia a la cámara para movimiento relativo.
    [SerializeField] private Transform cameraTransform;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    public bool IsGrounded { get; private set; }

    [Header("Movimiento")]
    // Velocidad de movimiento del personaje.
    [SerializeField] private float moveSpeed = 5f;
    // Velocidad base guardada para poder resetear el multiplicador.
    private float _baseMoveSpeed;
    // Velocidad horizontal actual del personaje.
    public Vector3 CurrentHorizontalVelocity { get; private set; }
    // Magnitud de la velocidad horizontal.
    public float CurrentSpeed { get; private set; }
    // Dirección actual del movimiento en el plano XZ.
    public Vector3 CurrentMoveDirection { get; private set; }

    private void Start()
    {
        if (playerInputController == null)
        {
            Debug.LogError("[PlayerMovementModel] Falta asignar PlayerInputController en el Inspector.");
        }
        if (rb == null)
        {
            Debug.LogError("[PlayerMovementModel] Falta asignar Rigidbody en el Inspector.");
        }
        if (groundCheck == null)
        {
            Debug.LogError("[PlayerMovementModel] Falta asignar GroundCheck en el Inspector.");
        }
        if (cameraTransform == null)
        {
            Debug.LogError("[PlayerMovementModel] Falta asignar CameraTransform en el Inspector.");
        }

        _baseMoveSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        CheckGround();
        Move();
        HandleJump();
        UpdateVelocityData();
    }

    private void CheckGround()
    {
        if (groundCheck == null) return;

        IsGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        Debug.Log($"[PlayerMovementModel] IsGrounded: {IsGrounded}");
    }

    private void HandleJump()
    {
        if (playerInputController == null || rb == null) return;

        if (playerInputController.JumpInput && IsGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log($"[PlayerMovementModel] Salto aplicado con fuerza: {jumpForce}");
            playerInputController.ConsumeJump();
        }
    }

    public void Move()
    {
        if (playerInputController == null || rb == null || cameraTransform == null) return;

        Vector2 input = playerInputController.MoveInput;

        // Calculamos direcciones relativas a la cámara.
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Convertimos el input 2D a dirección 3D relativa a la cámara.
        Vector3 moveDirection = forward * input.y + right * input.x;

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        if (moveDirection != Vector3.zero)
        {
            CurrentMoveDirection = moveDirection;
        }

        Vector3 newVelocity = new Vector3(
            moveDirection.x * moveSpeed,
            rb.linearVelocity.y,
            moveDirection.z * moveSpeed
        );

        rb.linearVelocity = newVelocity;

        if (moveDirection != Vector3.zero)
        {
            Debug.Log($"[PlayerMovementModel] Dirección de movimiento: {CurrentMoveDirection}");
            Debug.Log($"[PlayerMovementModel] Velocidad aplicada al Rigidbody: {rb.linearVelocity}");
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        moveSpeed = _baseMoveSpeed * multiplier;
        Debug.Log($"[PlayerMovementModel] Velocidad ajustada a: {moveSpeed}");
    }

    private void UpdateVelocityData()
    {
        if (rb == null) return;

        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0f;
        CurrentHorizontalVelocity = horizontalVelocity;
        CurrentSpeed = horizontalVelocity.magnitude;

        if (CurrentSpeed > 0f)
        {
            Debug.Log($"[PlayerMovementModel] CurrentHorizontalVelocity: {CurrentHorizontalVelocity} | CurrentSpeed: {CurrentSpeed}");
        }
    }
}