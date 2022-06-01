using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
* script que se encarga de analizar el texto ingresado en el input
* básicamente se encarga de:
*   limpiar el texto que recibe a caracteres que esperamos tener
*   separar cada palabra en su propio nodo
*   identificar si alguna palabra esta en el diccionario de conocidos 
*       (esto significa que, si tenemos una animación para la palabra 
*        para que en vez de deletrearlo en señas sea reconocible para una sola palabra)
*   enviar el listado de palabra para ser usado en el programa
* Cosas que se le podría agregar:
*   posibilidad de identificar frases con espacios
*   una opción que exija solo palabras conocidas o que todo sea para deletreo
*/

public class AnalizadorPalabras : MonoBehaviour
{
    /* Valores que se definen en la pantalla de
     * edición de Unity de los cuales son componentes
     * ---
     * variable                 Objetivo
     * textoInput               Es el cuadro de texto (Input) que contiene el 
                                escrito que se desea traducir.
     * listaPalabrasEncontradas Este no es una lista sino un componente script 
                                que controla lo físico del listado de nodos.
     * reproductor              Funciona como referencia del reproductor del 
                                personaje y básicamente de aquí se manda las 
                                palabras que deseamos ser animados.
     * enDixionario             Lista de texto que funciona para definir las 
                                palabras conocida que tiene animación
     */
    [SerializeField] private TMP_InputField textoInput;
    [SerializeField] private ListaANodo listaPalabrasEncontradas;
    [SerializeField] protected ReproductorSenhas reproductor;

    public List<string> enDixionario;
    
    //elemento publico de input para poder sacar el texto del elemento.
    public string valueInputIn
    {
        get { if (textoInput != null) return textoInput.text; return ""; }
    }
    
    //Funcion para el ebento del boton que inicie el proceso de analizar texto.
    public void OnAnalizarTexto()
    {
        AnalizarTexto();
    }

    //en este update revisa si se presiono enter
    //tambien agrege unos atajos de tecla para que escribiese 
    //  texto para probar ciertos casos, para ver si no hay errores
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

    /* algoritmo general de este método
     * se guarda el input que este en el texto
     * se limpia el dicho texto
     * se separan las palabras guardándolo en un array
     * se despliega para verlas en pantalla
     * enviar las palabras al reproductor
     */
    virtual protected void AnalizarTexto()
    {
        string respuesta;
        respuesta = valueInputIn;
        respuesta = LipiarTexto(respuesta);
        string[] palabrasSeparadas;
        palabrasSeparadas = SepararPalabras(respuesta);
        DesplegarPalabras(palabrasSeparadas);
        reproductor.RecivirPalabrasEncontradas(palabrasSeparadas, EnDiccionario(palabrasSeparadas));
    }

    //quitar elementos que no consideramos para el analizador
    virtual protected string LipiarTexto(string textoIn)
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

    protected string[] SepararPalabras(string textoCompleto)
    {
        if (textoCompleto == "")
            return new string[0];
        return textoCompleto.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    protected int[] EnDiccionario(string[] palabras)
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

    virtual protected void DesplegarPalabras(string[] palabras)
    {
        if (listaPalabrasEncontradas == null)
            return;
        listaPalabrasEncontradas.AderirNodos(palabras, EnDiccionario(palabras));
    }

    private void MostrarAbecedario()
    {
        string abecedario = "abcdefghijklmnñopqrstuvwxyz";
        if (textoInput != null)
            textoInput.text = abecedario;
    }

    protected void TextoEjemplo(string ejemplo)
    {
        if (textoInput != null)
            textoInput.text = ejemplo;
    }
}
