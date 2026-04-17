using UnityEngine;  // Librería principal de Unity.

public class PlayerAnimatorView : MonoBehaviour
{
    // Enum para representar el estado lógico actual de animación.
    public enum AnimationState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Hurt  // NUEVO
    }

    [Header("Referencias")]

    // Referencia al modelo de movimiento.
    [SerializeField] private PlayerMovementModel playerMovementModel;

    // Referencia al Animator del personaje.
    [SerializeField] private Animator animator;

    [SerializeField] private Transform characterVisual;

    [Header("Parámetro del Animator")]

    // Nombre del parámetro float del Animator.
    [SerializeField] private string speedParameter = "Speed";

    // Nombre del parámetro bool del Animator para el salto.
    [SerializeField] private string jumpParameter = "IsJumping";

    // NUEVO: Nombre del parámetro trigger del Animator para el daño.
    [SerializeField] private string hurtParameter = "IsHurt";

    [Header("Umbrales")]

    // Umbral para considerar Idle.
    [SerializeField] private float idleThreshold = 0.1f;

    // Umbral para considerar Run.
    [SerializeField] private float runThreshold = 4f;

    [Header("Rotación")]

    // Qué tan rápido gira el personaje.
    [SerializeField] private float rotationSpeed = 5f;

    // Estado actual detectado.
    public AnimationState CurrentState { get; private set; }

    // Hash del parámetro Speed.
    private int _speedHash;

    // Hash del parámetro IsJumping.
    private int _jumpHash;

    // NUEVO: Hash del parámetro IsHurt.
    private int _hurtHash;

    private void Start()
    {
        // Convertimos los nombres de parámetros a hash.
        _speedHash = Animator.StringToHash(speedParameter);
        _jumpHash = Animator.StringToHash(jumpParameter);
        _hurtHash = Animator.StringToHash(hurtParameter); // NUEVO

        // Revisamos referencias.
        if (playerMovementModel == null)
        {
            Debug.LogError("[PlayerAnimatorView] Falta asignar PlayerMovementModel en el Inspector.");
        }

        if (animator == null)
        {
            Debug.LogError("[PlayerAnimatorView] Falta asignar Animator en el Inspector.");
        }

        if (characterVisual == null)
        {
            Debug.LogError("[PlayerAnimatorView] Falta asignar Character Visual en el Inspector.");
        }

        Debug.Log($"[PlayerAnimatorView] Parámetro de Animator configurado: {speedParameter}");
    }

    private void Update()
    {
        // Actualizamos animación.
        UpdateAnimation();

        // Actualizamos rotación visual.
        UpdateRotation();
    }

    private void UpdateAnimation()
    {
        // Si falta algo, salimos.
        if (playerMovementModel == null || animator == null) return;

        // Tomamos la velocidad actual desde el modelo.
        float speed = playerMovementModel.CurrentSpeed;

        // La enviamos al Animator.
        animator.SetFloat(_speedHash, speed);

        // Enviamos si está saltando al Animator.
        bool isJumping = !playerMovementModel.IsGrounded;
        animator.SetBool(_jumpHash, isJumping);

        // Debug de Animator.
        if (speed > 0f)
        {
            Debug.Log($"[PlayerAnimatorView] Speed enviada al Animator: {speed}");
            Debug.Log($"[PlayerAnimatorView] Speed leída dentro del Animator: {animator.GetFloat(_speedHash)}");
        }

        // Determinamos el estado lógico actual.
        if (isJumping)
        {
            CurrentState = AnimationState.Jump;
        }
        else if (speed <= idleThreshold)
        {
            CurrentState = AnimationState.Idle;
        }
        else if (speed < runThreshold)
        {
            CurrentState = AnimationState.Walk;
        }
        else
        {
            CurrentState = AnimationState.Run;
        }

        // Debug del estado.
        Debug.Log($"[PlayerAnimatorView] Estado actual detectado: {CurrentState}");
    }

    // NUEVO: Se llama desde el evento OnDamageTaken del HealthSystem.
    public void PlayHurtAnimation()
    {
        if (animator == null) return;

        animator.SetTrigger(_hurtHash);
        CurrentState = AnimationState.Hurt;
        Debug.Log("[PlayerAnimatorView] Animación de daño activada.");
    }

    private void UpdateRotation()
    {
        // Si falta una referencia, no seguimos.
        if (playerMovementModel == null || characterVisual == null) return;

        // Tomamos la dirección actual del movimiento.
        Vector3 moveDirection = playerMovementModel.CurrentMoveDirection;

        // Si no hay dirección, no rotamos.
        if (moveDirection == Vector3.zero) return;

        // Calculamos la rotación objetivo basada en la dirección.
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        // Rotamos suavemente hacia la dirección deseada.
        characterVisual.rotation = Quaternion.Slerp(
            characterVisual.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        // Debug útil para confirmar rotación.
        Debug.Log($"[PlayerAnimatorView] Rotando hacia: {moveDirection}");
    }
}