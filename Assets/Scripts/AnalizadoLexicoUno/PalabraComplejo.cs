using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalabraComplejo
{
    public string texto;
    public int tipo;
    public int linea;
    public int columna;

    public PalabraComplejo()
    {
        this.texto  = "nada";
        this.tipo   = 0;
    }
    public PalabraComplejo(string palabra)
    {
        this.texto  = palabra;
        this.tipo   = 1;
    }
    public PalabraComplejo(string palabra, int suTipo)
    {
        this.texto  = palabra;
        this.tipo   = suTipo;
    }

    public void SetPosition(int lin, int col) 
    {
        this.linea = lin;
        this.columna = col;
    }

    public Palabra ComoPalabra(){
        return new Palabra(texto,tipo);
    }
}