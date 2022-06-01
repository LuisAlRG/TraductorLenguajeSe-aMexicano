using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

/*
* script que funciona para evitar que ciertos caracteres 
* sean usado en el recuadro de texto
*/

public class LimpiadorInput : MonoBehaviour
{
    [SerializeField] InputField principalInput;

    public bool letras = true;
    public bool numeros = false;
    public bool punto = false;
    public bool signos = false;

    private void OnEnable()
    {
        principalInput.onValueChanged.AddListener(InputValueChange);
    }

    private void OnDisable()
    {
        principalInput.onValueChanged.RemoveAllListeners();
    }

    private void Start()
    {
        principalInput = GetComponent<InputField>();
    }

    private string CleanInput(string strIn)
    {
        string quePermite = QueSeAdmite();
        // Replace invalid characters with empty strings.
        /*return Regex.Replace(strIn,
			  @"[^a-zñA-ZÑ0-9¡!¿?. ]", "");*/
        return Regex.Replace(strIn, quePermite, "");
    }

    private string QueSeAdmite()
    {
        string resultado = " ";
        if (letras)
            resultado += "a-zñA-ZÑ";
        if (numeros)
            resultado += "0-9";
        if (punto)
            resultado += ".";
        if (signos)
            resultado += "¿?¡!";
        resultado = @$"[^{resultado}]";
        return resultado;

    }

    void InputValueChange(string valorIntentado)
    {
        principalInput.text = CleanInput(valorIntentado);
    }

    void InputValueEnter()
    {
        Debug.Log("si Entro a value enter");
    }
}
