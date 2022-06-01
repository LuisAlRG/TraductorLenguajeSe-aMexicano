using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum estado
{
    NoPalabra,
    EsPalabra
}
public class AnalizadorComplejo 
{
    private string todoTexto = "";
    private List<PalabraComplejo> palabrasEncontradas;
    private int puntero = 0;
    private int ultimoSeguro = 0;
    private string palabraDelMomento = "";
    private estado suEstado;

    public AnalizadorComplejo(string texto)
    {
        this.todoTexto = texto;
        suEstado = estado.NoPalabra;
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
        PalabraComplejo p = new PalabraComplejo(palabraDelMomento);
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

    protected List<PalabraComplejo> AnalisarTexto()
    {
        for(puntero = 0;puntero < todoTexto.Length ; puntero++)
        {
            procesar();
        }
        return null;
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
            default:
                UnError(1);
                break;
        }
    }

    //caso NoPalabra
    private void CasoNoPalabra()
    {
        char caracter = ConsegirChar();
        if(EsLetra(caracter))
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
        if(EsLetra(caracter))
        {
            AderirLetraPalabra(caracter);
        }
        else if (EsNumero(caracter))
        {
            AderirLetraPalabra(caracter);
        }
        else if (EsGuion(caracter))
        {
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
            return;
        }
    }

    //caracteres de interes
    private bool EsLetra(char p) => char.IsLetter(p);

    private bool EsNumero(char p) => char.IsDigit(p);

    private bool EsEspacio(char p) => p == ' ';

    private bool EsGuion(char p) => p == '-' || p == '-';
}