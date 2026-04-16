using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject menuPausa;

    //variables
    [SerializeField] public static int puntos = 0;
    [SerializeField] int _vida = 10;
    [SerializeField] float _tiempo = 60f;
    [SerializeField] int _tiempoE;
    [SerializeField] public bool llave;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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

        }

    }

    public void RestarVida(int daño)
    {
        _vida -= daño;
        if (_vida <= 0)
        {
            SceneManager.LoadScene("Quit");
        }
    }

    public void SumarVida(int cura)
    {
        _vida += cura;
    }
}
