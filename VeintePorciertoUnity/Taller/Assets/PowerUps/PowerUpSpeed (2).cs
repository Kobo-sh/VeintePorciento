using UnityEngine;
using System.Collections;

public class PowerUpSpeed : MonoBehaviour
{
    [Header("Speed Power-Up Settings")]
    [SerializeField] private float speedMultiplier = 5f;
    [SerializeField] private float duration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats pStats = other.GetComponent<PlayerStats>();

            if (pStats != null)
            {
                pStats.ApplySpeedBoostTemporary(speedMultiplier, duration);
            }

            Destroy(gameObject);
        }
    }
}
