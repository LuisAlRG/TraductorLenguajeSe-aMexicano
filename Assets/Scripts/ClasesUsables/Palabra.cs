using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palabra
{
    public string texto;
    public int tipo;

    public Palabra()
    {
        this.texto  = "nada";
        this.tipo   = 0;
    }
    public Palabra(string palabra)
    {
        this.texto  = palabra;
        this.tipo   = 1;
    }
    public Palabra(string palabra, int suTipo)
    {
        this.texto  = palabra;
        this.tipo   = suTipo;
    }
}
