using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputAction moveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("move");
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        Debug.Log("Vector de movimiento " + moveValue); 
        
    }

    public Vector2 DireccionJugador()
    {
        return moveAction.ReadValue<Vector2>();

    }
 
}
