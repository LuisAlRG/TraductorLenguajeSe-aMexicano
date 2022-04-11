using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class AnalizadorPalabras : MonoBehaviour
{
    [SerializeField] private InputField textoInput;
    [SerializeField] private ListaANodo listaPalabrasEncontradas;
    [SerializeField] private ReproductorSenhas reproductor;

    public List<string> enDixionario;
    string valueInputIn
    {
        get { if (textoInput != null) return textoInput.text; return ""; }
    }
    
    public void OnAnalizarTexto()
    {
        AnalizarTexto();
    }
    private void Update()
    {
        if(
            Input.GetKeyUp(KeyCode.KeypadEnter)||
            Input.GetKeyUp(KeyCode.Return)
            )
        {
            AnalizarTexto();
        }

        if (Input.GetKeyUp(KeyCode.Alpha0))
            MostrarAbecedario();
        else if (Input.GetKeyUp(KeyCode.Alpha1))
            TextoEjemplo("Hola mi nombre es Luis");
        else if (Input.GetKeyUp(KeyCode.Alpha2))
            TextoEjemplo("palabras en minusculas PALABRAS EN MAYUSCULAS");
        else if (Input.GetKeyUp(KeyCode.Alpha3))
            TextoEjemplo("jkzxñ");
        else if (Input.GetKeyUp(KeyCode.Alpha4))
            TextoEjemplo("aba bab");
    }

    private void AnalizarTexto()
    {
        string respuesta;
        respuesta = valueInputIn;
        respuesta = LipiarTexto(respuesta);
        string[] palabrasSeparadas;
        palabrasSeparadas = SepararPalabras(respuesta);
        DesplegarPalabras(palabrasSeparadas);
        reproductor.RecivirPalabrasEncontradas(palabrasSeparadas, EnDixionario(palabrasSeparadas));
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

    private string[] SepararPalabras(string textoCompleto)
    {
        if (textoCompleto == "")
            return new string[0];
        return textoCompleto.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    private int[] EnDixionario(string[] palabras)
    {
        if(palabras == null)
            return new int[0];
        int[] resultado = new int[palabras.Length];
        for(int i = 0; palabras.Length > i; i++)
        {
            resultado[i] = palabraEnDixionario(palabras[i])? 1:2;
        }
        return resultado;
    }

    private bool palabraEnDixionario(string palabra)
    {
        if (enDixionario == null)
            return false;
        return enDixionario.Contains(palabra.ToUpper());
    }

    private void DesplegarPalabras(string[] palabras)
    {
        if (listaPalabrasEncontradas == null)
            return;
        listaPalabrasEncontradas.AderirNodos(palabras, EnDixionario(palabras));
    }

    private void MostrarAbecedario()
    {
        string abecedario = "abcdefghijklmnñopqrstuvwxyz";
        if (textoInput != null)
            textoInput.text = abecedario;
    }

    private void TextoEjemplo(string ejemplo)
    {
        if (textoInput != null)
            textoInput.text = ejemplo;
    }
}
