using UnityEngine;

/// <summary>
/// Sistema de colisión y daño para juegos 3D en Unity.
/// Adjuntar este script al objeto que CAUSA el daño (enemigo, trampa, proyectil, etc.).
/// Se comunica directamente con el componente HealthSystem del objeto golpeado.
/// </summary>
public class DamageOnCollision : MonoBehaviour
{
    // ──────────────────────────────────────────────
    // CONFIGURACIÓN EN EL INSPECTOR
    // ──────────────────────────────────────────────

    [Header("Daño")]
    [SerializeField] private float damageAmount = 20f;
    [SerializeField] private string targetTag   = "Player"; // Tag del objeto que recibirá daño

    [Header("Tipo de detección")]
    [SerializeField] private DetectionMode detectionMode = DetectionMode.CollisionAndTrigger;

    [Header("Daño continuo (zona de daño)")]
    [SerializeField] private bool  continuousDamage           = false;
    [SerializeField] private float continuousDamageInterval   = 1f; // Segundos entre cada tick de daño

    [Header("Comportamiento tras colisión")]
    [SerializeField] private bool  destroyOnHit               = false; // Útil para proyectiles
    [SerializeField] private float destroyDelay               = 0f;
    [SerializeField] private bool  disableAfterHit            = false; // Desactiva sin destruir
    [SerializeField] private int   maxHits                    = -1;    // -1 = ilimitado

    [Header("Efectos")]
    [SerializeField] private GameObject hitVFXPrefab;                  // Partículas al impactar
    [SerializeField] private AudioClip  hitSFX;                        // Sonido al impactar

    // ──────────────────────────────────────────────
    // VARIABLES PRIVADAS
    // ──────────────────────────────────────────────

    private int       hitCount          = 0;
    private float     damageTimer       = 0f;
    private bool      isInsideTrigger   = false;
    private AudioSource audioSource;

    // ──────────────────────────────────────────────
    // ENUMERACIÓN DE MODOS
    // ──────────────────────────────────────────────

    private enum DetectionMode
    {
        CollisionOnly,
        TriggerOnly,
        CollisionAndTrigger
    }

    // ──────────────────────────────────────────────
    // CICLO DE VIDA DE UNITY
    // ──────────────────────────────────────────────

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleContinuousDamage();
    }

    // ──────────────────────────────────────────────
    // COLISIONES FÍSICAS (Collider sin Is Trigger)
    // ──────────────────────────────────────────────

    private void OnCollisionEnter(Collision collision)
    {
        if (detectionMode == DetectionMode.TriggerOnly) return;
        if (!collision.gameObject.CompareTag(targetTag)) return;

        HealthSystem target = collision.gameObject.GetComponent<HealthSystem>();
        if (target == null) return;

        ApplyDamage(target, collision.contacts[0].point);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Solo usado si el daño continuo está activo y el modo lo permite
        if (!continuousDamage) return;
        if (detectionMode == DetectionMode.TriggerOnly) return;
        if (!collision.gameObject.CompareTag(targetTag)) return;

        isInsideTrigger = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag(targetTag)) return;
        isInsideTrigger = false;
        damageTimer = 0f;
    }

    // ──────────────────────────────────────────────
    // TRIGGERS (Collider con Is Trigger activado)
    // ──────────────────────────────────────────────

    private void OnTriggerEnter(Collider other)
    {
        if (detectionMode == DetectionMode.CollisionOnly) return;
        if (!other.CompareTag(targetTag)) return;

        HealthSystem target = other.GetComponent<HealthSystem>();
        if (target == null) return;

        if (!continuousDamage)
            ApplyDamage(target, other.ClosestPoint(transform.position));
        else
            isInsideTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!continuousDamage) return;
        if (detectionMode == DetectionMode.CollisionOnly) return;
        if (!other.CompareTag(targetTag)) return;

        isInsideTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;
        isInsideTrigger = false;
        damageTimer = 0f;
    }

    // ──────────────────────────────────────────────
    // MÉTODOS PRIVADOS
    // ──────────────────────────────────────────────

    /// <summary>
    /// Gestiona el daño por tick cuando el jugador está dentro de una zona de daño.
    /// </summary>
    private void HandleContinuousDamage()
    {
        if (!continuousDamage || !isInsideTrigger) return;

        damageTimer += Time.deltaTime;

        if (damageTimer >= continuousDamageInterval)
        {
            damageTimer = 0f;

            // Busca el objetivo dentro del área (trigger o colisión)
            Collider[] hits = Physics.OverlapSphere(transform.position,
                GetComponent<Collider>() ? GetComponent<Collider>().bounds.extents.magnitude : 1f);

            foreach (Collider hit in hits)
            {
                if (!hit.CompareTag(targetTag)) continue;

                HealthSystem target = hit.GetComponent<HealthSystem>();
                if (target != null)
                    ApplyDamage(target, hit.ClosestPoint(transform.position));
            }
        }
    }

    /// <summary>
    /// Aplica el daño al HealthSystem objetivo y ejecuta efectos visuales/sonoros.
    /// </summary>
    private void ApplyDamage(HealthSystem target, Vector3 hitPoint)
    {
        if (!CanHit()) return;

        target.TakeDamage(damageAmount);
        hitCount++;

        SpawnHitVFX(hitPoint);
        PlayHitSFX();

        Debug.Log($"[DamageOnCollision] {gameObject.name} causó {damageAmount} de daño a {target.gameObject.name}. " +
                  $"Golpes totales: {hitCount}");

        HandlePostHitBehavior();
    }

    /// <summary>
    /// Valida si este objeto aún puede causar daño según el límite de golpes.
    /// </summary>
    private bool CanHit()
    {
        if (maxHits < 0) return true;
        return hitCount < maxHits;
    }

    private void SpawnHitVFX(Vector3 position)
    {
        if (hitVFXPrefab == null) return;
        GameObject vfx = Instantiate(hitVFXPrefab, position, Quaternion.identity);
        Destroy(vfx, 3f); // Limpieza automática del efecto
    }

    private void PlayHitSFX()
    {
        if (hitSFX == null || audioSource == null) return;
        audioSource.PlayOneShot(hitSFX);
    }

    private void HandlePostHitBehavior()
    {
        if (destroyOnHit)
        {
            Destroy(gameObject, destroyDelay);
            return;
        }

        if (disableAfterHit && maxHits > 0 && hitCount >= maxHits)
        {
            gameObject.SetActive(false);
        }
    }

    // ──────────────────────────────────────────────
    // MÉTODOS PÚBLICOS ÚTILES
    // ──────────────────────────────────────────────

    /// <summary>
    /// Cambia el daño desde otro script en tiempo de ejecución.
    /// Útil para escalar dificultad o power-ups enemigos.
    /// </summary>
    public void SetDamage(float newDamage) => damageAmount = Mathf.Max(0f, newDamage);

    /// <summary>
    /// Resetea el contador de golpes (para objetos reutilizables).
    /// </summary>
    public void ResetHitCount() => hitCount = 0;

    // ──────────────────────────────────────────────
    // GIZMOS (visualización en el Editor)
    // ──────────────────────────────────────────────

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.35f);
        Collider col = GetComponent<Collider>();
        if (col != null)
            Gizmos.DrawCube(col.bounds.center, col.bounds.size);
    }
#endif
}
