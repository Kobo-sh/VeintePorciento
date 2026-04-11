using UnityEngine;
using TMPro;

public class PowerUpUI : MonoBehaviour
{

    public enum PowerUpType
    {
        Heal,
        SpeedBoost,
        Shield,
        DamageBoost
    }

    [Header("References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TMP_InputField valueInput;
    [SerializeField] private TMP_Text messageText;


    private PowerUpType selectedPowerUp;



    public void SelectHeal() => SetSelectedPowerUp(PowerUpType.Heal);

    public void SelectSpeedBoost() => SetSelectedPowerUp(PowerUpType.SpeedBoost);

    public void SelectShield() => SetSelectedPowerUp(PowerUpType.Shield);

    public void SelectDamageBoost() => SetSelectedPowerUp(PowerUpType.DamageBoost);

    private void SetSelectedPowerUp(PowerUpType type)
    {
        selectedPowerUp = type;
        messageText.text = $"Seleccionado: {selectedPowerUp}";
    }



    public void ApplySelectedPowerUp()
    {
        if (!ValidateReferences()) return;


        if (selectedPowerUp == PowerUpType.Shield)
        {
            if (!ValidateRules(1f)) return;

            ApplyPowerUp(1f);
            return;
        }

        if (!TryReadValue(out float value)) return;

        if (!ValidateRules(value)) return;

        ApplyPowerUp(value);
    }



    private bool ValidateReferences()
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats no asignado.");
            return false;
        }

        if (valueInput == null || messageText == null)
        {
            Debug.LogError("Referencias UI no asignadas.");
            return false;
        }

        return true;
    }

    private bool TryReadValue(out float value)
    {
        if (!float.TryParse(valueInput.text, out value))
        {
            messageText.text = "Error: Ingresa un número válido.";
            return false;
        }

        return true;
    }

    private bool ValidateRules(float value)
    {
        if (value <= 0f)
        {
            messageText.text = "Error: El valor debe ser mayor que 0.";
            return false;
        }

        switch (selectedPowerUp)
        {
            case PowerUpType.Heal:
                if (playerStats.CurrentHealth >= playerStats.MaxHealth)
                {
                    messageText.text = "Ya tienes la vida al máximo.";
                    return false;
                }
                break;

            case PowerUpType.Shield:
                if (playerStats.IsShieldActive)
                {
                    messageText.text = "El escudo ya está activo.";
                    return false;
                }
                break;
        }

        return true;
    }


    private void ApplyPowerUp(float value)
    {
        switch (selectedPowerUp)
        {
            case PowerUpType.Heal:
                playerStats.Heal(value);
                messageText.text = $"Curado. Vida actual: {playerStats.CurrentHealth}";
                break;

            case PowerUpType.SpeedBoost:
                playerStats.SetSpeedMultiplier(value);
                messageText.text = $"Velocidad actual: {playerStats.CurrentSpeed}";
                break;

            case PowerUpType.Shield:
                playerStats.SetShield(true);
                messageText.text = "Escudo activado.";
                break;

            case PowerUpType.DamageBoost:
                playerStats.TakeDamage(value);
                messageText.text = $"Recibiste {value} de daño. Vida actual: {playerStats.CurrentHealth}";
                break;
        }
    }
}