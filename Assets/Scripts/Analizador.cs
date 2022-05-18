using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

//Un intento pasado del script del Analizador de texto.

public class Analizador : MonoBehaviour
{
    public InputField inputField;
    public GameObject textDisplay;
    public GameObject reproductorAnimacion;
    void Start()
    {
            
    }
    void Update()
    {
        if (
            Input.GetKeyUp(KeyCode.KeypadEnter) ||
            Input.GetKeyUp(KeyCode.Return)
            )
        {
            OnAnalizarTexto();
        }
    }

    /*
     * | Posible proseso del Analisador de texto |
     *  Agregar el texto del cuadro de dialogo
     *  Revisar si esta vacio
     *      Salir
     *  Separar por espacio (consiguiendo lista)
     *  Recorrer listado
     *      Detectar si es conocido
     *          aderir la animacion conocida
     *      Casode no ser conocido
     *          Recorrer palabra letra por letra
     *              Aderir animacion por letra
     *  Reproducir la animacion (o tenerlo listo para la accion)
     */
    public void OnAnalizarTexto()
    {
        string respuesta;
        respuesta = ObtenerTexto();
        respuesta = LipiarTexto(respuesta);
        string[] palabrasSeparadas;
        palabrasSeparadas = SepararPalabras(respuesta);
        DesplegarTexto(palabrasSeparadas);
        if(reproductorAnimacion != null)
        {
            ReproductorAnimacion elementoScript = reproductorAnimacion.GetComponent<ReproductorAnimacion>();
            elementoScript.RecivirInformacion(palabrasSeparadas);
        }

    }

    //Evento que se acciona al precionar el boton alfabeto.
    public void OnDesplegarAlfabeto()
    {
        if (inputField == null)
            return;
        string abecedario = "abcdefghijklmnopqrstuvwxyzñ";
        inputField.text = abecedario;
    }

    //funcion que substrae el texto del input.
    private string ObtenerTexto()
    {
        if (inputField == null)
            return "";
        return inputField.textComponent.text;
    }

    //funcion de mostrar el texto en el otro "Text" box (usado para ver los elementos encontrados)
    private void DesplegarTexto(string texto)
    {
        if (textDisplay == null)
            return;
        textDisplay.GetComponent<Text>().text = texto;
    }

    //funcion de mostrar el texto en el otro "Text" box esta vez con el arreglo de string
    private void DesplegarTexto(string[] texto)
    {
        if (textDisplay == null)
            return;
        string enlistado= "Son:\n";
        for(int i=0; i<texto.Length; i++)
        {
            enlistado += texto[i];
            if(i+1 < texto.Length)
                enlistado += "/";
        }
        textDisplay.GetComponent<Text>().text = enlistado;
    }

    //quitar elementos que no consideramos para el analizador
    private string LipiarTexto(string textoIn)
    {
        //replasar simbolos por espacios
        textoIn = textoIn.Replace(",", " ");
        textoIn = textoIn.Replace("/", " ");
        textoIn = textoIn.Replace("\\", " ");
        //remplasar punto como nada
        textoIn = textoIn.Replace(".", "");
        //remplazar espacios blancos de mas en un solo espacio
        textoIn = Regex.Replace(textoIn, @"\s+", " ");

        return textoIn;
    }

    //funcion que separa todas las palabras encontrado en un texto
    private string[] SepararPalabras(string TextoCompleto)
    {
        if(TextoCompleto == "")
            return new string[0];
        return TextoCompleto.Split(' ');
    }
}
