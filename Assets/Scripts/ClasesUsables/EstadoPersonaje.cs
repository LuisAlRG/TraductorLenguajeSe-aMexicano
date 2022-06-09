using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Clase de Estado de personaje
* Esta clase funciona para indicar que estado esta nuestro personaje en pantalla,
* si esta en espera a que el usuario ponga play, si esta apunto de animar una palabra,
* si esta deletreando o animando completo y otros mas.
* En la forma en que funciona esto es que esta clase es creada en el script de reproductor
* de seña y esta clase llama a la función "handleInput" en el Update del programa, esta función
* revisa algún parámetro o tiempo en la que este el personaje en pantalla y regresa el estado que
* debería estar al detectar algo, si no detecta nada se regresa a si mismo, si detecta algo 
* regresa el siguiente estado que deseamos estar.
* los estados que tenemos son:
*   EnEspera -> espera a que el valor de "continuo" sea verdadero para empezar la reporducción
*   Deletreo -> almacena la palabra en reproducción y recorre cada letra para deletrearlo en pantalla,
*               también tiene que esperar a que la reproducción acabe (también tiene un tiempo de espera
*               para la siguiente animación de deletreo).
*   EjecutandoAnimacion -> guarda la palabra y ejecuta la animación de la palabra. esta sujeto a cambios.
*/
public class EstadoAtualPersonaje
{
    //instancia, basicamente es en donde la clase existe o deveria existir.
    protected ReproductorSenhas instancia;

    //inicializador
    public EstadoAtualPersonaje() { }

    //inicializador con parametro de la instancia pera estar seguro.
    public EstadoAtualPersonaje(ReproductorSenhas suInstancia)
    {
        this.instancia = suInstancia;
    }

    //Metodo que se llamara a cada momento del update de su instancia, basicamente se encarga de 
    //revisar si algun parametro cambio en el momento
    virtual public EstadoAtualPersonaje handleInput()
    {
        return this;
    }
}

public class EstadoEnEspera : EstadoAtualPersonaje
{
    private bool puroDeletreo = true;
    public EstadoEnEspera() { }

    public EstadoEnEspera(ReproductorSenhas suInstancia, bool puroDeletreo = true)
        : base(suInstancia)
    {
        this.puroDeletreo = puroDeletreo;
    }

    /*
     * 1.- revisar si esta activo el boton de reproduccion
     *  1.s si esta activo empieza la reproduccion de palabra
     * 2.- regresar el mismo estado de espera
     */
    public override EstadoAtualPersonaje handleInput()
    {
        if(instancia.continuo || instancia.ReproducirVeces())
        {
            return ReproducirPalabra();
        }
        return this;
    }

    /* 1.- consegir la palabra actual
     * 2.- revisar si lo tenemos definido (o si no lo queremos deletreado)
     *  2.s si esta definido hace animacion directa de seña (salida 2)
     * 3.- entra al deletreo (salida 1)
     */
    public EstadoAtualPersonaje ReproducirPalabra()
    {
        
        Palabra actual = this.instancia.ObtenerPalabraActual();
        if (actual.tipo == 1 && !puroDeletreo)
            return new EstadoEjecutandoAnimacion(this.instancia, actual.texto, 0);
        return new EstadoDeletreo(this.instancia, actual.texto);
    }
}

public class EstadoDeletreo : EstadoAtualPersonaje
{
    /*variable      Objetivo
     *palabra       mantiene la palabra entero en string
     *letraActual   que letra esta apuntando ahora mismo el puntero
     *puntero       en que letra del estring estamos por ahora
     *milisegundos  tiempo de espera base (sacado de su propia instancia)
     *retraso       tiempo de espera que controla cuanto tiempo a pasado
     */
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

    /* 1.- obtiene estado de animacion del personaje
     * 2.- obtiene si no esta trasicionando a otra animacion
     * 3.- revisa si esta en medio de animacion y el retraso inpuesto no a acabado
     *  3.s aplica siguiente paso
        3.1 revisa si no es final de palabra
         3.1.s detiene los parametros de animacion y sigue con la siguiente palabra
        3.2 sige con la siguiente letra
     * 4.- revisa si todavia hay retraso
     *  4.s reduce el retraso con el tiempo que paso el fotograma
     * 5.- regresar e mismo estado
     */
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

    // Combierte la letra a un nupero en especifico al no ser el formato comun
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
        string palabraSeñalada = palabra.Remove(puntero, 1).Insert(puntero, "[" + letraActual + "]");
        instancia.MostrarLetraActual(palabraSeñalada, letraActual);
        MostrarLetraAnimacion(ObtenerNumeroLetra(letraActual));
        SiguienteLetra();
    }
}

public class EstadoEjecutandoAnimacion : EstadoAtualPersonaje
{
    /*variable      Objetivo
     *palabra       mantiene la palabra entero en string
     *milisegundos  tiempo de espera base (sacado de su propia instancia)
     *retraso       tiempo de espera que controla cuanto tiempo a pasado
     */
    private string  palabra;
    private int     idAnimacion;
    private int     milisegundos;
    private float   retraso;

    public EstadoEjecutandoAnimacion(ReproductorSenhas suInstancia) : base(suInstancia)
    {
        this.palabra = "nada";
        this.idAnimacion = 0;
    }

    public EstadoEjecutandoAnimacion(ReproductorSenhas suInstancia, string palabraSent, int numero) : base(suInstancia)
    {
        this.palabra = palabraSent;
        this.idAnimacion = numero;
        milisegundos = suInstancia.miliSegundoEspera;
        retraso = 0f;
        suInstancia.PrepararParaDeletreo();
        suInstancia.MostrarAnimacionPalabra(numero);
        instancia.MostrarPalabraActual(palabraSent);
    }

    /* 1.- obtiene estado de animacion del personaje
     * 2.- obtiene si no esta trasicionando a otra animacion
     * 3.- revisa si esta en medio de animacion y el retraso inpuesto no a acabado
     *  3.s aplica siguiente paso
        3.1 limpiar animacion y regresar a espera.
     * 4.- revisa si todavia hay retraso
     *  4.s reduce el retraso con el tiempo que paso el fotograma
     * 5.- regresar e mismo estado
     */
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
            instancia.DetenerPalabra();
            instancia.LimpiarNombreActualAnimacion();
            if (instancia.continuo)
                    instancia.MoverSiguientePalabra();
            return new EstadoEnEspera(instancia);
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
}