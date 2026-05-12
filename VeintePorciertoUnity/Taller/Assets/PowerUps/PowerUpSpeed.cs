using UnityEngine;


public class PowerUpSpeed : MonoBehaviour
{
    public PlayerStats PStats; 
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PStats.SetSpeedMultiplier(5f);
            PStats.baseSpeed = 5;
            PStats.speedMultiplier = 5;
            PStats.CurrentSpeed 
            Destroy(gameObject);
        }

        

    }
}
