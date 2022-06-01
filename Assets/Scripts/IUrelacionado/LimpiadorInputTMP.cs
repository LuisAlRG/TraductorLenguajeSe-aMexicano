using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
* Script que funciona para evitar que ciertos caracteres 
* sean usado en el recuadro de texto, en este caso usando
* el input de TMP (Text Mesh Pro) como cuadro de texto.
*/

public class LimpiadorInputTMP : MonoBehaviour
{
    //input que requeriremos limpiar
    [SerializeField] TMP_InputField principalInput;

    //que cosas le queremos permitir.
    public bool letras = true;
    public bool numeros = false;
    public bool punto = false;
    public bool signos = false;
    public bool saltosDeLinea = false;

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
        principalInput = GetComponent<TMP_InputField>();
    }

    private string CleanInput(string strIn)
    {
        string quePermite = QueSeAdmite();
        // Replace invalid characters with empty strings.
        /*return Regex.Replace(strIn,
              @"[^a-zñA-ZÑ0-9¿?. ]", "");*/
        return Regex.Replace(strIn, quePermite, "");
    }

    private string QueSeAdmite()
    {
        string resultado = " ";
        if (letras)
            resultado += "a-záéíóúñA-ZÁÉÍÓÚÑ";
        if (numeros)
            resultado += "0-9";
        if (punto)
            resultado += ".";
        if (signos)
            resultado += "¿?¡!";
        if (saltosDeLinea)
            resultado += "\n";
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
