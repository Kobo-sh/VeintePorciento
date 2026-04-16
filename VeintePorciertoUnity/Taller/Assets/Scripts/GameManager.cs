using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private float delayBeforeGameOver = 2f;

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

    public void RestartGame()
    {
        gameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}