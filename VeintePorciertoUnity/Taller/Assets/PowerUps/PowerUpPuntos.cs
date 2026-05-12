using UnityEngine;



public class PowerUpPuntos : MonoBehaviour
{
    public PlayerStats PStats;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PStats.puntos += 1;
            Destroy(gameObject); 
        }



    }
}
