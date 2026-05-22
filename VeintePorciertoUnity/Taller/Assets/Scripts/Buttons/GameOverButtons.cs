using UnityEngine;

public class GameOverButtons : MonoBehaviour
{
    public void OnCheckpoint()
    {
        GameManager.Instance.EstadoDelJuego("Checkpoint");
    }

    public void OnReiniciar()
    {
        GameManager.Instance.EstadoDelJuego("Reset");
    }

    public void OnSalir()
    {
        GameManager.Instance.EstadoDelJuego("Menu");
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
    }
}