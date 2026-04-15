using UnityEngine;  // Librería principal de Unity.
public class PlayerMovementModel : MonoBehaviour
{
    [Header("Referencias")]
    // Referencia al script que lee el input.
    [SerializeField] private PlayerInputController playerInputController;
    // Referencia al Rigidbody del personaje.
    [SerializeField] private Rigidbody rb;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    public bool IsGrounded { get; private set; }

    [Header("Movimiento")]
    // Velocidad de movimiento del personaje.
    [SerializeField] private float moveSpeed = 5f;
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
        // Validamos groundCheck.
        if (groundCheck == null)
        {
            Debug.LogError("[PlayerMovementModel] Falta asignar GroundCheck en el Inspector.");
        }
    }

    private void FixedUpdate()
    {
        // Verificamos si está en el suelo.
        CheckGround();
        // Mueve el rigidbody.
        Move();
        // Maneja el salto.
        HandleJump();
        // Actualiza velocidad y dirección real.
        UpdateVelocityData();
    }

    // NUEVO: Detecta si el personaje está tocando el suelo.
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

    // NUEVO: Aplica la fuerza de salto si hay input y está en el suelo.
    private void HandleJump()
    {
        if (playerInputController == null || rb == null) return;

        if (playerInputController.JumpInput && IsGrounded)
        {
            // Reseteamos Y para que el salto sea siempre consistente.
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log($"[PlayerMovementModel] Salto aplicado con fuerza: {jumpForce}");

            //fix del delay
            playerInputController.ConsumeJump();
        }
    }

    public void Move()
    {
        if (playerInputController == null || rb == null) return;

        Vector2 input = playerInputController.MoveInput;
        Vector3 moveDirection = new Vector3(input.x, 0f, input.y);

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