using UnityEngine;


public class PowerUpHeal : MonoBehaviour
{
    private HealthSystem healPU; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healPU.Heal(50f);
            Destroy(gameObject);
        }

        

    }
}
