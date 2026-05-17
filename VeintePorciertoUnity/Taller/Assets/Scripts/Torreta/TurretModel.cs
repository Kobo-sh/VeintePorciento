using UnityEngine;

public class TurretModel : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private float detectionRange = 10f;

    [Header("Disparo")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 1.5f;

    [Header("Vida")]
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    public float DetectionRange => detectionRange;
    public float Damage => damage;
    public float FireRate => fireRate;
    public float CurrentHealth => currentHealth;
    public bool IsAlive => currentHealth > 0f;

    public enum TurretState { Idle, Attacking }
    public TurretState CurrentState { get; set; }

    private void Awake()
    {
        currentHealth = maxHealth;
        CurrentState = TurretState.Idle;
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;
        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        Debug.Log($"[TurretModel] Vida restante: {currentHealth}");

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        Debug.Log("[TurretModel] Torreta destruida.");
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}