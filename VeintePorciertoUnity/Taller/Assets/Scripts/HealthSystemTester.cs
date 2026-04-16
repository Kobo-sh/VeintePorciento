using UnityEngine;

/// <summary>
/// Script de ejemplo para probar el sistema de vida.
/// Adjuntarlo al jugador. Demuestra daño por colisión y curación por tecla.
/// </summary>
[RequireComponent(typeof(HealthSystem))]
public class HealthSystemTester : MonoBehaviour
{
    // ──────────────────────────────────────────────
    // CONFIGURACIÓN EN EL INSPECTOR
    // ──────────────────────────────────────────────

    [Header("Pruebas rápidas (teclas)")]
    [SerializeField] private KeyCode damageKey = KeyCode.X;
    [SerializeField] private KeyCode healKey   = KeyCode.Z;
    [SerializeField] private KeyCode killKey   = KeyCode.K;
    [SerializeField] private KeyCode reviveKey = KeyCode.R;

    [Header("Valores de prueba")]
    [SerializeField] private float testDamageAmount = 20f;
    [SerializeField] private float testHealAmount   = 25f;

    [Header("Daño por colisión")]
    [SerializeField] private string damageTag       = "Hazard"; // Tag del objeto dañino
    [SerializeField] private float  collisionDamage = 10f;

    // ──────────────────────────────────────────────
    // VARIABLES PRIVADAS
    // ──────────────────────────────────────────────

    private HealthSystem healthSystem;

    // ──────────────────────────────────────────────
    // CICLO DE VIDA DE UNITY
    // ──────────────────────────────────────────────

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        // Subscribirse a eventos para reacciones adicionales
        healthSystem.OnDeath.AddListener(HandleDeath);
        healthSystem.OnDamageTaken.AddListener(HandleDamageTaken);
        healthSystem.OnRevive.AddListener(HandleRevive);
    }

    private void Update()
    {
        HandleDebugInput();
    }

    // ──────────────────────────────────────────────
    // COLISIONES 3D
    // ──────────────────────────────────────────────

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(damageTag))
        {
            healthSystem.TakeDamage(collisionDamage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ejemplo: recoger un item de curación
        if (other.CompareTag("HealthPickup"))
        {
            healthSystem.Heal(testHealAmount);
            Destroy(other.gameObject);
        }
    }

    // ──────────────────────────────────────────────
    // CALLBACKS DE EVENTOS
    // ──────────────────────────────────────────────

    private void HandleDeath()
    {
        Debug.Log("[Tester] ¡El jugador ha muerto! Activar animación de muerte, pantalla de game over, etc.");
        // Aquí puedes llamar a: GameManager.Instance.GameOver();
        // O cargar una escena: SceneManager.LoadScene("GameOver");
    }

    private void HandleDamageTaken(float amount)
    {
        Debug.Log($"[Tester] Daño recibido: {amount}. Activar feedback (pantalla roja, sonido, etc.)");
        // Ejemplo: StartCoroutine(FlashDamageEffect());
    }

    private void HandleRevive()
    {
        Debug.Log("[Tester] ¡El jugador revivió!");
    }

    // ──────────────────────────────────────────────
    // INPUT DE PRUEBA (solo en desarrollo)
    // ──────────────────────────────────────────────

    private void HandleDebugInput()
    {
        if (Input.GetKeyDown(damageKey))
        {
            Debug.Log($"[Tester] Tecla presionada: aplicar {testDamageAmount} de daño.");
            healthSystem.TakeDamage(testDamageAmount);
        }

        if (Input.GetKeyDown(healKey))
        {
            Debug.Log($"[Tester] Tecla presionada: curar {testHealAmount}.");
            healthSystem.Heal(testHealAmount);
        }

        if (Input.GetKeyDown(killKey))
        {
            Debug.Log("[Tester] Tecla presionada: muerte instantánea.");
            healthSystem.InstantKill();
        }

        if (Input.GetKeyDown(reviveKey))
        {
            Debug.Log("[Tester] Tecla presionada: revivir.");
            healthSystem.Revive();
        }
    }
}
