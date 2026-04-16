using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject menuPausa;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
}
