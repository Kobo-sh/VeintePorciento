using System.Collections;
using Unity.VisualScripting;
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

    [Header("Checkpoints")]
    private Checkpoint lastCheckpoint;
    private Vector3 lastCheckpointPosition;
    private bool hasCheckpoint = false;

    private bool gameOver = false;
    private string currentScene;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "PapiPerdiste" && scene.name != "MenuInicial")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            HealthSystem playerHealth = GameObject.FindWithTag("Player")?.GetComponent<HealthSystem>();
            if (playerHealth != null)
                playerHealth.OnDeath.AddListener(OnPlayerDeath);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        HealthSystem playerHealth = GameObject.FindWithTag("Player")?.GetComponent<HealthSystem>();
        if (playerHealth != null)
            playerHealth.OnDeath.AddListener(OnPlayerDeath);
        else
            Debug.LogWarning("[GameManager] No se encontró HealthSystem en el Player.");

        if (SceneManager.GetActiveScene().name == "MenuInicial" ||
            SceneManager.GetActiveScene().name == "PapiPerdiste")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case "Nivel1":
                gameOver = false;
                Time.timeScale = 1;
                SceneManager.LoadScene("Nvl1");
                break;
            case "Nivel2":
                gameOver = false;
                Time.timeScale = 1;
                SceneManager.LoadScene("Nvl2");
                break;
            case "Pause":
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case "Checkpoint":
                gameOver = false;
                Time.timeScale = 1;
                StartCoroutine(LoadAndRespawn());
                break;
            case "Quit":
                Application.Quit();
                break;
            case "Reset":
                gameOver = false;
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                SceneManager.LoadScene(currentScene);
                break;
            case "Menu":
                gameOver = false;
                Time.timeScale = 1;
                hasCheckpoint = false;
                lastCheckpoint = null;
                SceneManager.LoadScene("MenuInicial");
                break;
        }
    }

    public void OnPlayerDeath()
    {
        if (gameOver) return;
        gameOver = true;
        currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("[GameManager] El jugador murió. Cargando pantalla de Game Over...");
        StartCoroutine(LoadGameOverScene());
    }

    private IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(delayBeforeGameOver);
        SceneManager.LoadScene("PapiPerdiste");
    }

    private IEnumerator LoadAndRespawn()
    {
        SceneManager.LoadScene(currentScene);
        yield return new WaitForSeconds(0.5f);
        RespawnPlayer();
    }

    public void RegisterCheckpoint(Checkpoint checkpoint)
    {
        if (lastCheckpoint == checkpoint) return;

        if (lastCheckpoint != null)
            lastCheckpoint.Deactivate();

        lastCheckpoint = checkpoint;
        lastCheckpointPosition = checkpoint.transform.position;
        hasCheckpoint = true;
        lastCheckpoint.Activate();

        Debug.Log($"[GameManager] Checkpoint registrado: {checkpoint.gameObject.name}");
    }

    public void RespawnPlayer()
    {
        if (!hasCheckpoint)
        {
            Debug.LogWarning("[GameManager] No hay checkpoint registrado.");
            return;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = lastCheckpointPosition + Vector3.up;
            player.GetComponent<HealthSystem>()?.HealFull();
        }

        Debug.Log("[GameManager] Jugador respawneado en checkpoint.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collision"))
        {
        }
    }
}