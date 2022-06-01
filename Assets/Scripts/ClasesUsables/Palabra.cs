using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Clase que funciona para guardar que tipo de palabra se esta usando 
* (si es conocida y tenemos su animación), tiene la palabra en string y 
* un int que indica si lo conocemos o no
* esta sujeto a cambios.
*/

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
