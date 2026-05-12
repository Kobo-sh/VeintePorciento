using UnityEngine;

public class PowerUpShield : MonoBehaviour
{
    public PlayerStats PStats;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PStats.SetShield(true);
           
            Destroy(gameObject);
        }



    }
}
