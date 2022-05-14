using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoAtualPersonaje
{
    protected ReproductorSenhas instancia;

    public EstadoAtualPersonaje() { }

    public EstadoAtualPersonaje(ReproductorSenhas suInstancia)
    {
        this.instancia = suInstancia;
    }

    virtual public EstadoAtualPersonaje handleInput()
    {
        return this;
    }
}

public class EstadoEnEspera : EstadoAtualPersonaje
{
    public EstadoEnEspera() { }

    public EstadoEnEspera(ReproductorSenhas suInstancia)
        : base(suInstancia)
    {

    }

    public override EstadoAtualPersonaje handleInput()
    {
        if(instancia.continuo || instancia.ReproducirVeces())
        {
            return ReproducirPalabra();
        }
        return this;
    }

    public EstadoAtualPersonaje ReproducirPalabra()
    {
        /* 1.- consegir la palabra actual
         * 2.- revisar si lo tenemos definido
         */
        Palabra actual = this.instancia.ObtenerPalabraActual();
        if (actual.tipo == 1)
            return new EstadoEjecutandoAnimacion(this.instancia, actual.texto, 0);
        return new EstadoDeletreo(this.instancia, actual.texto);
    }
}

public class EstadoDeletreo : EstadoAtualPersonaje
{
    private string  palabra;
    private char    letraActual;
    private int     puntero;
    private int     milisegundos;
    private float   retraso;

    public EstadoDeletreo(ReproductorSenhas suInstancia, string palabraEnv)
        : base(suInstancia)
    {
        palabra = palabraEnv;
        puntero = 0;
        milisegundos = suInstancia.miliSegundoEspera;
        retraso = 0f;
        instancia.PrepararParaDeletreo();
    }
    public override EstadoAtualPersonaje handleInput()
    {
        float estado = instancia.EstaEnAnimacion();
        bool enTrancicion = instancia.EnTrancicion();
        if ( 
            estado>1 &&
            retraso < 1f &&
            !enTrancicion
            )
        {
            if (FinPalabra())
            {
                instancia.DetenerDeletreo();
                instancia.LimpiarNombreActualAnimacion();
                if (instancia.continuo)
                    instancia.MoverSiguientePalabra();
                return new EstadoEnEspera(instancia);
            }
            Proceso();
        }
        if(
            estado > 1 &&
            retraso > 0
            )
        {
            retraso = retraso - (Time.deltaTime * 1_000);
        }
        return this;
    }

    private bool FinPalabra()
    {
        return palabra.Length <= puntero;
    }

    private void SiguienteLetra()
    {
        puntero++;
    }

    private void ObtenerLetra()
    {
        letraActual = palabra[puntero];
    }

    private int ObtenerNumeroLetra(char letra)
    {
        int respuesta = 0;
        if (letra == 'ñ' || letra == 'Ñ')
            respuesta = 27;
        else if (letra >= 'a' && letra <= 'z')
            respuesta = letra - 'a' + 1;
        if (letra >= 'A' && letra <= 'Z')
            respuesta = letra - 'A' + 1;
        if (letra == 'á' || letra == 'Á')
            respuesta = 1;
        if (letra == 'é' || letra == 'É')
            respuesta = 5;
        if (letra == 'í' || letra == 'Í')
            respuesta = 9;
        if (letra == 'ó' || letra == 'Ó')
            respuesta = 15;
        if (letra == 'ú' || letra == 'Ú')
            respuesta = 21;
        return respuesta;
    }

    private void MostrarLetraAnimacion(int numero)
    {
        this.instancia.MostrarLetraDeletreo(numero);
        retraso = milisegundos;
    }

    private void Proceso()
    {
        ObtenerLetra();
        //aver algo
        string palabraSeñalada = palabra.Remove(puntero, 1).Insert(puntero, "[" + letraActual + "]");

        instancia.MostrarLetraActual(palabraSeñalada, letraActual);
        MostrarLetraAnimacion(ObtenerNumeroLetra(letraActual));
        SiguienteLetra();
    }
}

public class EstadoEjecutandoAnimacion : EstadoAtualPersonaje
{
    private string palabra;
    private int idAnimacion;

    public EstadoEjecutandoAnimacion(ReproductorSenhas suInstancia) : base(suInstancia)
    {
        this.palabra = "nada";
        this.idAnimacion = 0;
    }

    public EstadoEjecutandoAnimacion(ReproductorSenhas suInstancia, string palabraSent, int numero) : base(suInstancia)
    {
        this.palabra = palabraSent;
        this.idAnimacion = numero;
    }

    public override EstadoAtualPersonaje handleInput()
    {
        return this;
    }
}