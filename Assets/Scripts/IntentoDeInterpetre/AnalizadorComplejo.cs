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
        for(;puntero < todoTexto.Length ; puntero++)
        {

        }
        return null;
    }    

    private void procesar(char letra)
    {
        switch(suEstado)
        {
            case estado.NoPalabra:

                break;
            case estado.EsPalabra:

                break;
            default:
                UnError(1);
                break;
        }
    }

    //caracteres de interes
    private bool EsLetra(char p) => char.IsLetter(p);

    private bool EsNumero(char p) => char.IsDigit(p);

    private bool EsEspacio(char p) => p == ' ';

    private bool EsGuion(char p) => p == '-' || p == '-';
}