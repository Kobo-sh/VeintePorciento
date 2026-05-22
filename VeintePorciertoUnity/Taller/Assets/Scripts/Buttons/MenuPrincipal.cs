using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
    }

    public void Botonesmenu(string accion)
    {
        switch (accion)
        {
            case "Nivel1":
                SceneManager.LoadScene("Nvl1");
                break;
            case "Nivel2":
                SceneManager.LoadScene("Nvl2");
                break;
            case "Salir":
                Application.Quit();
                break;
        }
    }
}