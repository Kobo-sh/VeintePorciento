using UnityEngine;
using UnityEngine.SceneManagement;

public class PasoButtons : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
    }

    public void AccionPaso(string accion)
    {
        switch (accion)
        {
            case "Nivel2":
                SceneManager.LoadScene("Nvl2");
                break;
            case "Menu":
                SceneManager.LoadScene("MenuInicial");
                break;
          
        }
    }
}