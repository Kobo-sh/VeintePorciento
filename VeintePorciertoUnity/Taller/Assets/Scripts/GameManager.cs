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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

    private IEnumerator LoadAndRespawn()
    {
        SceneManager.LoadScene("probuilder");
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        RespawnPlayer();
    }

    public void RegisterCheckpoint(Checkpoint checkpoint)
    {
        if (lastCheckpoint == checkpoint) return;

        if (lastCheckpoint != null)
            lastCheckpoint.Deactivate();

        lastCheckpoint = checkpoint;
        lastCheckpoint.Activate();

        Debug.Log($"[GameManager] Checkpoint registrado: {checkpoint.gameObject.name}");
    }

    public void RespawnPlayer()
    {
        if (lastCheckpoint == null)
        {
            Debug.LogWarning("[GameManager] No hay checkpoint registrado.");
            return;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = lastCheckpoint.transform.position + Vector3.up;
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