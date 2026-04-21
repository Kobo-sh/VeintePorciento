using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private PlayerMovementModel playerMovementModel;
    [SerializeField] private PlayerAnimatorView playerAnimatorView;

    [Header("Configuración")]
    [SerializeField] private float healAmount = 25f;
    [SerializeField] private float speedMultiplier = 2f;
    [SerializeField] private float speedDuration = 5f;
    [SerializeField] private float shieldDuration = 5f;
    [SerializeField] private float damageReduction = 0.5f; // 0.5 = 50% menos daño

    // Estado interno
    private bool isSpeedActive = false;
    private bool isShieldActive = false;
    private bool isDamageReductionActive = false;
    private float speedTimer = 0f;
    private float shieldTimer = 0f;
    private float damageTimer = 0f;
    private float originalSpeed;

    private void Start()
    {
        if (healthSystem == null)
            Debug.LogError("[PowerUpController] Falta asignar HealthSystem.");

        if (playerMovementModel == null)
            Debug.LogError("[PowerUpController] Falta asignar PlayerMovementModel.");

        if (playerAnimatorView == null)
            Debug.LogError("[PowerUpController] Falta asignar PlayerAnimatorView.");
    }

    private void Update()
    {
        HandleInput();
        HandleTimers();
    }

    private void HandleInput()
    {
        // E — Curación
        if (Keyboard.current.eKey.wasPressedThisFrame)
            ActivateHeal();

        // R — SpeedBoost
        if (Keyboard.current.rKey.wasPressedThisFrame)
            ActivateSpeedBoost();

        // F — Shield
        if (Keyboard.current.fKey.wasPressedThisFrame)
            ActivateShield();

        // G — Damage Reduction
        if (Keyboard.current.gKey.wasPressedThisFrame)
            ActivateDamageReduction();
    }

    private void HandleTimers()
    {
        // Timer SpeedBoost
        if (isSpeedActive)
        {
            speedTimer -= Time.deltaTime;
            if (speedTimer <= 0f)
                DeactivateSpeedBoost();
        }

        // Timer Shield
        if (isShieldActive)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0f)
                DeactivateShield();
        }

        // Timer Damage Reduction
        if (isDamageReductionActive)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
                DeactivateDamageReduction();
        }
    }

    // ──────────────────────────────────────────────
    // POWER UPS
    // ──────────────────────────────────────────────

    private void ActivateHeal()
    {
        if (healthSystem == null) return;
        healthSystem.Heal(healAmount);
        playerAnimatorView?.PlayHealAnimation();
        Debug.Log($"[PowerUpController] Curación activada: +{healAmount} vida.");
    }

    private void ActivateSpeedBoost()
    {
        if (playerMovementModel == null || isSpeedActive) return;
        playerMovementModel.SetSpeedMultiplier(speedMultiplier);
        isSpeedActive = true;
        speedTimer = speedDuration;
        Debug.Log($"[PowerUpController] SpeedBoost activado por {speedDuration}s.");
    }

    private void DeactivateSpeedBoost()
    {
        if (playerMovementModel == null) return;
        playerMovementModel.SetSpeedMultiplier(1f);
        isSpeedActive = false;
        Debug.Log("[PowerUpController] SpeedBoost terminado.");
    }

    private void ActivateShield()
    {
        if (healthSystem == null || isShieldActive) return;
        healthSystem.ActivateInvincibility(shieldDuration);
        isShieldActive = true;
        shieldTimer = shieldDuration;
        Debug.Log($"[PowerUpController] Escudo activado por {shieldDuration}s.");
    }

    private void DeactivateShield()
    {
        isShieldActive = false;
        Debug.Log("[PowerUpController] Escudo terminado.");
    }

    private void ActivateDamageReduction()
    {
        if (isDamageReductionActive) return;
        isDamageReductionActive = true;
        damageTimer = 5f;
        Debug.Log($"[PowerUpController] Reducción de daño activada: {damageReduction * 100}% por 5s.");
    }

    private void DeactivateDamageReduction()
    {
        isDamageReductionActive = false;
        Debug.Log("[PowerUpController] Reducción de daño terminada.");
    }

    // Llamado desde HealthSystem antes de aplicar daño
    public float ApplyDamageReduction(float incomingDamage)
    {
        if (!isDamageReductionActive) return incomingDamage;
        return incomingDamage * (1f - damageReduction);
    }
}