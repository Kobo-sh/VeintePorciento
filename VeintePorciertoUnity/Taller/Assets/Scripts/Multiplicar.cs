using UnityEngine;
using TMPro;

public class Multiplicar : MonoBehaviour
{

    [SerializeField] private TMP_InputField inputFieldNumero1;

    [SerializeField] private TMP_InputField inputFieldNumero2;

    [SerializeField] private TMP_Text _textResultado;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // MultiplicarNumeros(float.Parse(inputFieldNumero1.text, float parse(inputFieldNumero1));
;        
    }

    public float MultiplicarNumeros(float a, float b)
    {
        return a * b;

    }
}
