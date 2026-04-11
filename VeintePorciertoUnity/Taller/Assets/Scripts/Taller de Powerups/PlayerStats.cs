using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    [Header("Speed Settings")]
    [SerializeField] private float baseSpeed = 5f;
    private float speedMultiplier = 1f;

    [Header("Shield")]
    [SerializeField] private bool isShieldActive = false;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float CurrentSpeed => baseSpeed * speedMultiplier;
    public bool IsShieldActive => isShieldActive;



    public void Heal(float amount)
    {
        if (amount <= 0f)

            return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f)
            return;


        if (isShieldActive)
        {
            isShieldActive = false;
            Debug.Log("El escudo bloqueó el daño y se rompió");
            return;
        }


        currentHealth -= damage;

        if (currentHealth < 0f)
            currentHealth = 0f;

        Debug.Log("Vida actual: " + currentHealth);
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        if (multiplier <= 0f)

            return;

        speedMultiplier = multiplier;

        return;
    }

    public void SetShield(bool active)
    {
        isShieldActive = active;
    }
}