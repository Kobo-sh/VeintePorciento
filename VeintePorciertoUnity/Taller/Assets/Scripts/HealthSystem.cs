using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Sistema de vida funcional para juegos 3D en Unity.
/// Soporta daño, curación, invencibilidad temporal, regeneración y muerte.
/// Adjuntar este componente al GameObject del jugador o enemigo.
/// </summary>
public class HealthSystem : MonoBehaviour
{
    // ──────────────────────────────────────────────
    // CONFIGURACIÓN EN EL INSPECTOR
    // ──────────────────────────────────────────────

    [Header("Vida")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Regeneración")]
    [SerializeField] private bool enableRegeneration = false;
    [SerializeField] private float regenAmountPerSecond = 5f;
    [SerializeField] private float regenDelayAfterDamage = 3f;

    [Header("Invencibilidad temporal (iFrames)")]
    [SerializeField] private bool enableInvincibilityFrames = true;
    [SerializeField] private float invincibilityDuration = 1.0f;

    [Header("Muerte")]
    [SerializeField] private bool destroyOnDeath = false;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private GameObject deathVFXPrefab;

    // ──────────────────────────────────────────────
    // EVENTOS (asignables desde el Inspector)
    // ──────────────────────────────────────────────

    [Header("Eventos")]
    public UnityEvent<float, float> OnHealthChanged;  // (vidaActual, vidaMaxima)
    public UnityEvent<float>        OnDamageTaken;     // (cantidadDeDaño)
    public UnityEvent<float>        OnHealed;          // (cantidadCurada)
    public UnityEvent               OnDeath;
    public UnityEvent               OnRevive;

    // ──────────────────────────────────────────────
    // PROPIEDADES PÚBLICAS (solo lectura)
    // ──────────────────────────────────────────────

    public float CurrentHealth    => currentHealth;
    public float MaxHealth        => maxHealth;
    public bool  IsAlive          => currentHealth > 0f;
    public bool  IsInvincible     => isInvincible;
    public float HealthPercentage => currentHealth / maxHealth;

    // ──────────────────────────────────────────────
    // VARIABLES PRIVADAS
    // ──────────────────────────────────────────────

    private bool  isInvincible       = false;
    private float regenTimer         = 0f;
    private bool  isDead             = false;
    private Coroutine invincibilityCoroutine;
    private Coroutine regenCoroutine;

    // ──────────────────────────────────────────────
    // CICLO DE VIDA DE UNITY
    // ──────────────────────────────────────────────

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        HandleRegeneration();
    }

    // ──────────────────────────────────────────────
    // MÉTODOS PÚBLICOS PRINCIPALES
    // ──────────────────────────────────────────────

    /// <summary>
    /// Aplica daño al objeto. Respeta los iFrames si están activos.
    /// </summary>
    /// <param name="amount">Cantidad de daño (valor positivo).</param>
    public void TakeDamage(float amount)
    {
        if (!IsAlive || isInvincible) return;
        if (amount <= 0f) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        regenTimer = 0f; // Reinicia el contador de regeneración

        OnDamageTaken?.Invoke(amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (enableInvincibilityFrames)
            ActivateInvincibility(invincibilityDuration);

        Debug.Log($"[HealthSystem] {gameObject.name} recibió {amount} de daño. Vida: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f)
            Die();
    }

    /// <summary>
    /// Cura al objeto una cantidad determinada, sin superar la vida máxima.
    /// </summary>
    /// <param name="amount">Cantidad a curar (valor positivo).</param>
    public void Heal(float amount)
    {
        if (!IsAlive) return;
        if (amount <= 0f) return;

        float previousHealth = currentHealth;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        float actualHeal = currentHealth - previousHealth;

        OnHealed?.Invoke(actualHeal);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        Debug.Log($"[HealthSystem] {gameObject.name} se curó {actualHeal}. Vida: {currentHealth}/{maxHealth}");
    }

    /// <summary>
    /// Restaura la vida al máximo.
    /// </summary>
    public void HealFull()
    {
        Heal(maxHealth);
    }

    /// <summary>
    /// Activa invencibilidad temporal por una duración personalizada.
    /// </summary>
    public void ActivateInvincibility(float duration)
    {
        if (invincibilityCoroutine != null)
            StopCoroutine(invincibilityCoroutine);

        invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine(duration));
    }

    /// <summary>
    /// Mata al objeto de forma instantánea, independientemente de su vida actual.
    /// </summary>
    public void InstantKill()
    {
        if (!IsAlive) return;
        currentHealth = 0f;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Die();
    }

    /// <summary>
    /// Revive al objeto con una cantidad de vida especificada.
    /// </summary>
    /// <param name="healthOnRevive">Vida con la que revive (por defecto: vida máxima).</param>
    public void Revive(float healthOnRevive = -1f)
    {
        if (IsAlive) return;

        isDead = false;
        currentHealth = (healthOnRevive < 0f) ? maxHealth : Mathf.Clamp(healthOnRevive, 1f, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnRevive?.Invoke();

        Debug.Log($"[HealthSystem] {gameObject.name} ha sido revivido con {currentHealth} de vida.");
    }

    /// <summary>
    /// Modifica la vida máxima. Si adjustCurrent es true, escala la vida actual proporcionalmente.
    /// </summary>
    public void SetMaxHealth(float newMaxHealth, bool adjustCurrent = false)
    {
        if (newMaxHealth <= 0f) return;

        float ratio = currentHealth / maxHealth;
        maxHealth = newMaxHealth;

        if (adjustCurrent)
            currentHealth = Mathf.Round(ratio * maxHealth);
        else
            currentHealth = Mathf.Min(currentHealth, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // ──────────────────────────────────────────────
    // MÉTODOS PRIVADOS
    // ──────────────────────────────────────────────

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"[HealthSystem] {gameObject.name} ha muerto.");

        if (deathVFXPrefab != null)
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);

        OnDeath?.Invoke();

        if (destroyOnDeath)
            Destroy(gameObject, destroyDelay);
    }

    private void HandleRegeneration()
    {
        if (!enableRegeneration || !IsAlive || currentHealth >= maxHealth) return;

        regenTimer += Time.deltaTime;

        if (regenTimer >= regenDelayAfterDamage)
        {
            Heal(regenAmountPerSecond * Time.deltaTime);
        }
    }

    // ──────────────────────────────────────────────
    // CORRUTINAS
    // ──────────────────────────────────────────────

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        Debug.Log($"[HealthSystem] {gameObject.name} es invencible por {duration}s.");
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    // ──────────────────────────────────────────────
    // GIZMOS (visualización en el Editor)
    // ──────────────────────────────────────────────

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Muestra una barra de vida sobre el objeto en la Scene view
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position + Vector3.up * 2.2f, Vector3.up, 0.5f);
    }
#endif
}
