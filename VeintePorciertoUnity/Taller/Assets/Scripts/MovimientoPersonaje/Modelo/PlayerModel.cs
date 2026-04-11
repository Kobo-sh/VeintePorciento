using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]

    public Rigidbody rb;
    

    float velocidad = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Movimiento();
    }

    public void Movimiento()
    {
        rb.linearVelocity = playerController.DireccionJugador() * velocidad;  

    }
}
