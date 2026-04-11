using UnityEngine;
using TMPro;

public class CalculadoraUI : MonoBehaviour
{
    public enum Operador
    {
        Suma,
        Resta,
        Multiplicacion,
        Division
        
    }

    [Header("Inputs")]
    [SerializeField] private TMP_InputField _InputA;
    [SerializeField] private TMP_InputField _InputB;

    [Header("outputs")]
    [SerializeField] private TMP_Text _resultado;



    [SerializeField] private Operador _operador;

    public void SeleccionarOperador(Operador operador)
    {
        _operador = operador;

    }
    public void OperadorSuma() => _operador = Operador.Suma;
    public void OperadorResta() => _operador = Operador.Resta;
    public void OperadorMultiplicacion() => _operador = Operador.Multiplicacion;
    public void OperadorDivision() => _operador = Operador.Division;




    public void Calcular()
    { 
        if (!float.TryParse(_InputA.text, out float a) || !float.TryParse(_InputB.text, out float b))
        {
            _resultado.text = "Hola Therians";
            return;

        }

        
        switch (_operador)
        {
            case Operador.Suma:
                _resultado.text = ( a + b).ToString();
                break;
            case Operador.Resta:
                _resultado.text = ( a - b).ToString();
                break;
            case Operador.Multiplicacion:
                _resultado.text = ( a * b).ToString();
                break;
            case Operador.Division:

            if (b == 0)
            {
                _resultado.text = "Tonto";
                
            }
            else
            {
                _resultado.text = (a / b).ToString();

            }

                break;
            default:
               _resultado.text = "oprime un boton";

                break;

        }
    
    }



    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
