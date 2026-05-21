using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controla la barra de vida en la interfaz de usuario (UI).
/// Requiere un componente Slider o Image (tipo Filled) en la escena.
/// Conectar los eventos de HealthSystem a UpdateHealthBar() desde el Inspector.
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    // ──────────────────────────────────────────────
    // CONFIGURACIÓN EN EL INSPECTOR
    // ──────────────────────────────────────────────

    [Header("Referencia al sistema de vida")]
    [SerializeField] private HealthSystem healthSystem;

    [Header("UI - Barra")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthFillImage;

    [Header("UI - Texto")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private bool showAsPercentage = false;

    [Header("Colores dinámicos")]
    [SerializeField] private bool useDynamicColor = true;
    [SerializeField] private Color colorFull = Color.green;
    [SerializeField] private Color colorMid = Color.yellow;
    [SerializeField] private Color colorLow = Color.red;
    [SerializeField][Range(0f, 1f)] private float midThreshold = 0.5f;
    [SerializeField][Range(0f, 1f)] private float lowThreshold = 0.25f;

    [Header("Puntos")]
    [SerializeField] private TMP_Text puntosText;
    [SerializeField] private PlayerStats playerStats;

    // ──────────────────────────────────────────────
    // CICLO DE VIDA DE UNITY
    // ──────────────────────────────────────────────

    private void Start()
    {
        if (healthSystem == null)
        {
            Debug.LogWarning("[HealthBarUI] No se asignó un HealthSystem. Busca uno automáticamente.");
            healthSystem = FindObjectOfType<HealthSystem>();
        }

        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged.AddListener(UpdateHealthBar);
            UpdateHealthBar(healthSystem.CurrentHealth, healthSystem.MaxHealth);
        }
    }

    private void Update()
    {
        if (playerStats != null && puntosText != null)
            puntosText.text = $"Puntos: {playerStats.puntos}";
    }

    private void OnDestroy()
    {
        if (healthSystem != null)
            healthSystem.OnHealthChanged.RemoveListener(UpdateHealthBar);
    }

    // ──────────────────────────────────────────────
    // MÉTODOS PÚBLICOS
    // ──────────────────────────────────────────────

    public void UpdateHealthBar(float current, float max)
    {
        float percentage = (max > 0f) ? current / max : 0f;

        if (healthSlider != null)
            healthSlider.value = percentage;

        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = percentage;

            if (useDynamicColor)
                healthFillImage.color = GetHealthColor(percentage);
        }

        if (healthText != null)
        {
            healthText.text = showAsPercentage
                ? $"{Mathf.CeilToInt(percentage * 100)}%"
                : $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
        }
    }

    // ──────────────────────────────────────────────
    // MÉTODOS PRIVADOS
    // ──────────────────────────────────────────────

    private Color GetHealthColor(float percentage)
    {
        if (percentage <= lowThreshold)
            return colorLow;
        else if (percentage <= midThreshold)
            return Color.Lerp(colorLow, colorMid, (percentage - lowThreshold) / (midThreshold - lowThreshold));
        else
            return Color.Lerp(colorMid, colorFull, (percentage - midThreshold) / (1f - midThreshold));
    }
}