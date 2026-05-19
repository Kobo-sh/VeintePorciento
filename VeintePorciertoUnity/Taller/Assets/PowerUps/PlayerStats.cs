using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public int puntos = 0;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    [Header("Speed Settings")]
    public float baseSpeed = 5f;
    public float currentSpeed = 5f;

    [Header("Shield")]
    [SerializeField] private bool isShieldActive = false;

    [Header("Referencias")]
    // Referencia al script de movimiento real del personaje
    [SerializeField] private PlayerMovementModel playerMovementModel;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsShieldActive => isShieldActive;

    private Coroutine speedBoostCoroutine;

    private void Awake()
    {
        // Si no se asignó en el Inspector, lo buscamos en el mismo GameObject
        if (playerMovementModel == null)
            playerMovementModel = GetComponent<PlayerMovementModel>();
    }

    public void Heal(float amount)
    {
        if (amount <= 0f) return;
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f) return;
        if (isShieldActive)
        {
            isShieldActive = false;
            Debug.Log("El escudo bloqueó el daño y se rompió");
            return;
        }
        currentHealth -= damage;
        if (currentHealth < 0f) currentHealth = 0f;
        Debug.Log("Vida actual: " + currentHealth);
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        if (multiplier <= 0f) return;
        currentSpeed = baseSpeed * multiplier;

        // Aplicar al movimiento real del personaje
        if (playerMovementModel != null)
            playerMovementModel.SetSpeedMultiplier(multiplier);
        else
            Debug.LogWarning("[PlayerStats] No se encontró PlayerMovementModel. La velocidad no se aplicó al movimiento.");
    }

    public void SetShield(bool active)
    {
        isShieldActive = active;
    }

    /// <summary>
    /// Aplica un boost de velocidad temporal. Si ya hay uno activo,
    /// lo reinicia con los nuevos valores en lugar de apilar efectos.
    /// </summary>
    public void ApplySpeedBoostTemporary(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
            StopCoroutine(speedBoostCoroutine);

        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        SetSpeedMultiplier(multiplier);
        Debug.Log($"Speed boost activado: x{multiplier} por {duration} segundos.");

        yield return new WaitForSeconds(duration);

        SetSpeedMultiplier(1f);
        Debug.Log("Speed boost terminado. Velocidad restaurada.");

        speedBoostCoroutine = null;
    }
}
