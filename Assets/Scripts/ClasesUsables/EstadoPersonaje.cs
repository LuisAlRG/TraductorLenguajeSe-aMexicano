using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Clase de Estado de personaje
* Esta clase funciona para indicar que estado esta nuestro personaje en pantalla,
* si esta en espera a que el usuario ponga play, si esta apunto de animar una palabra,
* si esta deletreando o animando completo y otros mas.
* En la forma en que funciona esto es que esta clase es creada en el script de reproductor
* de seña y esta clase llama a la funcion "handleInput" en el Update del programa, esta funcion
* revisa algun parametro o tiempo en la que este el personaje en pantalla y regresa el estado que
* deveria estar al detectar algo, si no detecta nada se regresa a si mismo, si dectecta algo 
* regresa el siguiente estado que deseamos estar.
* los estados que tenemos son:
*   EnEspera -> espera a que el valor de "continuo" sea verdadero para empezar la reporduccion
*   Deletreo -> almacena la palabra en reproduccion y recorre cada letra para deletrearlo en pantalla,
*               tambien tiene que esperar a que la reproduccion acabe (tambien tiene un tiempo de espera
*               para lasiguiente animacion de deletreo).
*   EjecutandoAnimacion -> guarda la palabra y ejecuta la animacion de la palabra. esta sujeto a cambios.
*/
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
        if (letra == 'u' || letra == 'Ú')
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