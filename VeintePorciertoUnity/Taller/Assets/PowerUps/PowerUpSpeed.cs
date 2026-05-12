using UnityEngine;


public class PowerUpSpeed : MonoBehaviour
{
    public PlayerStats PStats; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PStats.SetSpeedMultiplier(5f);
            Destroy(gameObject);
        }

        

    }
}
