using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LimpiadorInputTMP : MonoBehaviour
{
    [SerializeField] TMP_InputField principalInput;

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
              @"[^a-zÒA-Z—0-9ø?. ]", "");*/
        return Regex.Replace(strIn, quePermite, "");
    }

    private string QueSeAdmite()
    {
        string resultado = " ";
        if (letras)
            resultado += "a-zÒ·ÈÌÛ˙A-Z—¡…Õ”⁄";
        if (numeros)
            resultado += "0-9";
        if (punto)
            resultado += ".";
        if (signos)
            resultado += "ø?°!";
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
