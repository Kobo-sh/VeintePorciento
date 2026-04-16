using System.Collections;
using UnityEditor.ProBuilder;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private float delayBeforeGameOver = 2f;
    [SerializeField] private GameObject menuPausa;

    private bool gameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        HealthSystem playerHealth = GameObject.FindWithTag("Player")?.GetComponent<HealthSystem>();
        if (playerHealth != null)
            playerHealth.OnDeath.AddListener(OnPlayerDeath);
        else
            Debug.LogWarning("[GameManager] No se encontró HealthSystem en el Player.");
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (menuPausa.activeSelf)
            {
                EstadoDelJuego("Play");
                menuPausa.SetActive(false);
            }
            else
            {
                EstadoDelJuego("Pause");
                menuPausa.SetActive(true);
            }
        }
    }

    public void EstadoDelJuego(string estado)
    {
        switch (estado)
        {
            case "Play":
                Time.timeScale = 1;
                break;
            case "Pause":
                Time.timeScale = 0;
                break;
            case "Quit":
                Application.Quit();
                break;
            case "Reset":
                gameOver = false;
                Time.timeScale = 1;
                SceneManager.LoadScene("probuilder");
                break;
        }
    }

    public void OnPlayerDeath()
    {
        if (gameOver) return;
        gameOver = true;
        Debug.Log("[GameManager] El jugador murió. Cargando pantalla de Game Over...");
        StartCoroutine(LoadGameOverScene());
    }

    private IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(delayBeforeGameOver);
        SceneManager.LoadScene("PapiPerdiste");
    }

  
}