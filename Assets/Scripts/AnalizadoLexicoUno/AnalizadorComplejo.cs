using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum estado
{
    NoPalabra,
    EsPalabra,
    GionPalabra
}
public class AnalizadorComplejo 
{
    private string todoTexto = "";
    private List<Palabra> palabrasEncontradas;
    private int puntero = 0;
    private int ultimoSeguro = 0;
    private string palabraDelMomento = "";
    private estado suEstado;

    public AnalizadorComplejo(string texto)
    {
        this.todoTexto = texto;
        suEstado = estado.NoPalabra;
        palabrasEncontradas = new List<Palabra>();
    }

    private char ConsegirChar()
    {
        return todoTexto[puntero];
    }

    private void SiguienteLetra()
    {
        puntero++;
    }

    private void AderirLetraPalabra(char cara)
    {
        palabraDelMomento += cara;
    }

    private void AderirPalabraLista()
    {
        Palabra p = new Palabra(palabraDelMomento);
        p.posicion = ultimoSeguro + 1;
        p.largo = puntero - ultimoSeguro -1;
        palabrasEncontradas.Add(p);
    }

    private void ResetearPalabra()
    {
        palabraDelMomento = "";
    }

    private void RegresarAlSeguro()
    {
        puntero = ultimoSeguro;
    }

    private void ActualizarSeguro()
    {
        ultimoSeguro = puntero;
    }

    private void UnError(int num){
        string mensaje = "Error num:" + num+" en la clase \"AnalizadorComplejo\"|";
        switch(num)
        {
            case 1:
                mensaje += "Estado No Reconosido";
                break;
            default:
                mensaje += "Error desconosido o el que diseño esto hiso algo pero que el error.";
                break;
        }
        Debug.Log(mensaje);
    }

    public List<Palabra> AnalisarTexto()
    {
        for(puntero = 0;puntero < todoTexto.Length ; puntero++)
        {
            procesar();
        }
        if(suEstado == estado.EsPalabra || suEstado == estado.GionPalabra){
            AderirPalabraLista();
            ActualizarSeguro();
            ResetearPalabra();
            suEstado = estado.NoPalabra;
        }

        return palabrasEncontradas;
    }    

    private void procesar()
    {
        switch(suEstado)
        {
            case estado.NoPalabra:
                CasoNoPalabra();
                break;
            case estado.EsPalabra:
                CasoEsPalabra();
                break;
            case estado.GionPalabra:
                CasoGionPalabra();
                break;
            default:
                UnError(1);
                break;
        }
    }

    //caso NoPalabra
    private void CasoNoPalabra()
    {
        char caracter = ConsegirChar();
        if(EsLetraMex(caracter))
        {
            AderirLetraPalabra(caracter);
            suEstado = estado.EsPalabra;
        }
        else if (EsNumero(caracter))
        {
            AderirLetraPalabra(caracter);
            suEstado = estado.EsPalabra;
        }
        else
        {
            ActualizarSeguro();
            return;
        }

    }

    //caso EsPalabra
    private void CasoEsPalabra()
    {
        char caracter = ConsegirChar();
        if(EsLetraMex(caracter))
        {
            AderirLetraPalabra(caracter);
        }
        else if (EsNumero(caracter))
        {
            AderirLetraPalabra(caracter);
        }
        else if (EsGuion(caracter))
        {
            suEstado = estado.GionPalabra;
            return;
        }
        else if (EsEspacio(caracter))
        {
            AderirPalabraLista();
            ActualizarSeguro();
            ResetearPalabra();
            suEstado = estado.NoPalabra;
        }
        else 
        {
            AderirPalabraLista();
            ActualizarSeguro();
            ResetearPalabra();
            suEstado = estado.NoPalabra;
        }
    }

    //Caso estamos en guion y tenemos que revisar si hay una letra al siguente espacio
    private void CasoGionPalabra(){
        char caracter = ConsegirChar();
        if(EsLetraMex(caracter))
        {
            AderirLetraPalabra(caracter);
            suEstado = estado.EsPalabra;
        }
        else if (EsNumero(caracter))
        {
            AderirLetraPalabra(caracter);
            suEstado = estado.EsPalabra;
        }
        //else if (EsGuion(caracter)){return;}
        //else if (EsEspacio(caracter)){return;}
        else 
        {
            return;
        }
    }

    //caracteres de interes
    private bool EsLetra(char p) => char.IsLetter(p);
    
    /* el metodo de "IsLetter" permiten algunas 
     * letras que no se encuentran en el alfabeto mexicano 
     * por lo que requiero uno especifico
     */
    private bool EsLetraMex(char p){
        string letrasMexicanas = 
        "abcdefghijklmnñopqrstuvwxyz" +
        "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ" +
        "áéíóúü" + 
        "ÁÉÍÓÚÜ" ;
        return letrasMexicanas.IndexOf(p) > -1;
    }

    private bool EsNumero(char p) => char.IsDigit(p);

    private bool EsEspacio(char p) => p == ' ';

    private bool EsGuion(char p) => p == '-' || p == '-';
}