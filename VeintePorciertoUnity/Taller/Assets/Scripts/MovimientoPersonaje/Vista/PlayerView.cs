using UnityEngine;

public class PlayerView : MonoBehaviour
{

    [SerializeField] private Renderer render;
    [SerializeField] private Material material1;
    [SerializeField] private Material material2;

    [Header("Clases")]
    [SerializeField] private PlayerController controller;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        render = GetComponent<Renderer>();

       //ontroller.DireccionJugador().magnitude;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
