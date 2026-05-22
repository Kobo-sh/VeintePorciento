using UnityEngine;

public class PausaButtons : MonoBehaviour
{
    public void Reanudar()
    {
        GameManager.Instance.EstadoDelJuego("Play");
        GameManager.Instance.CerrarMenuPausa();
    }

    public void Checkpoint()
    {
        GameManager.Instance.EstadoDelJuego("Checkpoint");
    }

    public void Reiniciar()
    {
        GameManager.Instance.EstadoDelJuego("Reset");
    }

    public void Menu()
    {
        GameManager.Instance.EstadoDelJuego("Menu");
    }
}