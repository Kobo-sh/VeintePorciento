using UnityEngine;

public class Funciones : MonoBehaviour
{

    private int _numeroEntero = 2;

    public Clases ListaEstudiantes = new Clases();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MyFunction();
    }

    // Update is called once per frame
    
    void Update()
    {
        //no saturar
    }

    public void MyFunction()
    {
        Debug.Log(ListaEstudiantes.estudiantes[2]);
        Debug.Log(ListaEstudiantes.edadEstudiantes[2]);
       Debug.Log(ListaEstudiantes.Suma(5 , 6));

    }
}
