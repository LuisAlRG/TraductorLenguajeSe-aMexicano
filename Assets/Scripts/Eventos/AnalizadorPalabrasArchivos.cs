using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using AnotherFileBrowser.Windows;
using TMPro;

/*
* script que se encarga de analisar el texto de un archivo Cargado.
* Basicamente se encarga de lo mismo que el script de "AnalizadorPalabra"
* pero tiene unas cosas extras:
*   Tiene otra manera de limpiar el texto (considera salto de linea y giones).
*   La posibilidad de cargar archivos de windows.
* Cosas que se le podria agregar:
*   Invetigando un poco los atces de abrir archivos, seria conseguir 
*       una manera de ver que funcione con otros sistrmas operativos.
*/

public class AnalizadorPalabrasArchivos : AnalizadorPalabras
{
    private string textoEnCuestion = "";

    public void OnLeerArchivo()
    {
        LeerArchivo();
        if (textoEnCuestion == "")
            return;
        AnalizarTexto();
    }

    public void OnProcesarTexto()
    {
        if(valueInputIn != "")
            textoEnCuestion = valueInputIn;
        if (textoEnCuestion == "")
            return;
        AnalizarTexto();
    }

    //Metodo de leer archivo con ayuda de las clases y pluging de abrir archivo
    private void LeerArchivo()
    {
        BrowserProperties vp = new BrowserProperties()
        {
            filter = "Documento Texto (*.txt) | *.txt",
            filterIndex = 0,
            initialDir = "C:\\Users\\Luisito\\Documents",
            title = "Seleccione un archivo texto"
        };

        new FileBrowser().OpenFileBrowser(vp, direccion =>
        {
            string textoResultante = "";
            foreach (string line in File.ReadLines(direccion))
            {
                textoResultante += line + "\n";
            }
            textoEnCuestion = textoResultante;
        });

        TextoEjemplo(textoEnCuestion);
    }

    private string QueLetrasSeAdmite()
    {
        string resultado = " ";
        //letras
            resultado += "a-zñA-ZÑ";
        //acento
            resultado += "áéíóú´ÁÉÍÓÚ";
        //numeros
        resultado += "0-9";
        resultado = @$"[^{resultado}]";
        return resultado;
    }

    protected override string LipiarTexto(string textoIn)
    {
        //replasar simbolos por espacios
        textoIn = textoIn.Replace(",", " ");
        textoIn = textoIn.Replace("/", " ");
        textoIn = textoIn.Replace("\\", " ");
        //remplazar espacios blancos de mas en un solo espacio
        textoIn = Regex.Replace(textoIn, @"\s+", " ");
        //ve si no tiene un gion al final que indica que es una palabra dividida en dos
        //textoIn = textoIn.Replace(" \n", "\n");
        textoIn = textoIn.Replace("- ","");
        Debug.Log(textoIn);
        //remplasar punto como nada
        textoIn = textoIn.Replace(".", "");

        // una ultima revision para que solo permita letras y numeros pero no simbolos extraños
        textoIn = Regex.Replace(textoIn, QueLetrasSeAdmite(), "");

        return textoIn;
    }


    protected override void AnalizarTexto()
    {
        string respuesta;
        respuesta = textoEnCuestion;
        respuesta = LipiarTexto(respuesta);
        string[] palabrasSeparadas;
        palabrasSeparadas = SepararPalabras(respuesta);
        DesplegarPalabras(palabrasSeparadas);
        reproductor.RecivirPalabrasEncontradas(palabrasSeparadas, EnDixionario(palabrasSeparadas));
    }

    protected override void DesplegarPalabras(string[] palabras)
    {
        base.DesplegarPalabras(palabras);
    }
}

