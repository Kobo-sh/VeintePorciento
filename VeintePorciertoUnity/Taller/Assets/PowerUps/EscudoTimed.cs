using System.Collections;
using UnityEngine;

public class PowerUpShieldTimed : MonoBehaviour
{
    [Header("Shield Settings")]
    [SerializeField] private float shieldDuration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats pStats = other.GetComponent<PlayerStats>();

            if (pStats == null)
            {
                Debug.LogWarning("El objeto Player no tiene un componente PlayerStats.");
                return;
            }

            if (pStats.IsShieldActive)
            {
                Debug.Log("El escudo ya está activo, no se puede recoger.");
                return;
            }

            pStats.StartCoroutine(ActivateShieldForDuration(pStats));
            Destroy(gameObject);
        }
    }

    private IEnumerator ActivateShieldForDuration(PlayerStats pStats)
    {
        pStats.SetShield(true);
        Debug.Log($"Escudo activado por {shieldDuration} segundos.");

        yield return new WaitForSeconds(shieldDuration);

        // Solo desactiva si el escudo sigue activo (puede haberse roto antes por daño)
        if (pStats.IsShieldActive)
        {
            pStats.SetShield(false);
            Debug.Log("Escudo desactivado por tiempo.");
        }
    }
}