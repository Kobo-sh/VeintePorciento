using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoriaButtons : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
    }

    public void AccionVictoria(string accion)
    {
        switch (accion)
        {
            case "Menu":
                SceneManager.LoadScene("MenuInicial");
                break;
            case "Creditos":
                SceneManager.LoadScene("Creditos");
                break;
        }
    }
}