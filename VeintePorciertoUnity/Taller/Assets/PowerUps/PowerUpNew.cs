using UnityEngine;


public class PowerUpNew : MonoBehaviour
{
    public HealthSystem healPU; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healPU.Heal(50f);
            Destroy(gameObject);
        }

        

    }
}
