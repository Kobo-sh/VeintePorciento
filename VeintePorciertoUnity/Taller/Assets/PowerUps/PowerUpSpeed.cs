using UnityEngine;
using System.Collections;

public class PowerUpSpeed : MonoBehaviour
{
    public PlayerStats PStats;

    [Header("Speed Power-Up Settings")]
    [SerializeField] private float speedMultiplier = 5f;
    [SerializeField] private float duration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Si PStats no está asignado en el Inspector, lo buscamos en el jugador
            if (PStats == null)
                PStats = other.GetComponent<PlayerStats>();

            if (PStats != null)
            {
                // Usamos un GameObject temporal para ejecutar la corrutina
                // ya que este objeto se destruye inmediatamente
                SpeedBoostRunner.Run(PStats, speedMultiplier, duration);
            }

            Destroy(gameObject);
        }
    }
}

/// <summary>
/// Helper estático que crea un GameObject temporal para ejecutar
/// la corrutina después de que el PowerUp sea destruido.
/// </summary>
public class SpeedBoostRunner : MonoBehaviour
{
    public static void Run(PlayerStats stats, float multiplier, float duration)
    {
        GameObject runner = new GameObject("SpeedBoostRunner");
        SpeedBoostRunner component = runner.AddComponent<SpeedBoostRunner>();
        component.StartCoroutine(component.ApplyBoost(stats, multiplier, duration));
    }

    private IEnumerator ApplyBoost(PlayerStats stats, float multiplier, float duration)
    {
        // Aplicar el boost
        stats.SetSpeedMultiplier(multiplier);
        Debug.Log($"Speed boost activado: x{multiplier} por {duration} segundos.");

        // Esperar el tiempo indicado
        yield return new WaitForSeconds(duration);

        // Revertir la velocidad al valor base (multiplicador = 1)
        stats.SetSpeedMultiplier(1f);
        Debug.Log("Speed boost terminado. Velocidad restaurada.");

        // Destruir el GameObject temporal
        Destroy(gameObject);
    }
}
