using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReproductorAnimacion : MonoBehaviour
{
    /*
     * Estos valores son los que se define en la pantalla de edicion de unity
     * ----------------------------------------------------------------------
     * Variable                 Objetivo
     * animador                 Es el controlador de animador que funciona para que el modelo del personaje se mueva
     * miliSegundos             Tiempo en que deja espacio entre animacion de deletreo
     * mostrarNombreAnimacion   Espacio para poner el elemento "Text" de unity para mostrar el nombre de la animacion 
     *                          que se esta reproduciendo, no pasa nada si no se define en unity.
     * 
     */
    public Animator animador;
    public int miliSegundos;
    public Text mostrarNombreAnimacion;

    /*
     * Estos valores son de caracter privados, solo se modifican en codigo
     * -------------------------------------------------------------------
     * palabrasEncontrados      Es un arreglo de las palabras escritas en el texto, se guarda cuando el objeto de
     *                          evento les pasa su resultado de su analisis.
     * puntero                  Este es lo que se usa para definir el puntero del arreglo de palabras, requerido para 
     *                          hacer la animacion paso a paso.
     * estadoPersonaje          Objeto que define en que estado esta el personaje que hace todas las animaciones, importante
     *                          para controlar bien los input que se requiera en el momento.
     * reproducirContinuo       Valor booleano que nos indica si la persona requiere que se ejecute de manera consecutiva las
     *                          palabras
     */
    private string [] palabrasEncontrados;
    private int puntero;
    private EstadoPersonje estadoPersonaje;
    private bool reproducirContinuo = false;

    //Por si a alguien se le ocurre como crear esto desde codigo, pueden usar los metodos de 
    //inisialisacion, aunque si ya esta enlasado con un objeto uniti no ara falta
    public ReproductorAnimacion()
    {
        palabrasEncontrados = new string [] {"Nada"};
        puntero = 0;
    }
    public ReproductorAnimacion(string [] palabrasPasadas)
    {
        this.palabrasEncontrados = palabrasPasadas;
        puntero = 0;
    }

    //Metodo que usamos para recivir el arreglo de palabras del primer objeto de evento
    public void RecivirInformacion(string[] palabrasPasadas)
    {
        this.palabrasEncontrados = palabrasPasadas;
        puntero = 0;
        estadoPersonaje = new EstadoEnEspera(this);
    }

    //Metodo para cambiar el valor de continuo (funciona para que se reproduscan consecutivamente todas las palabras)
    public void CambiarContinuo()
    {
        reproducirContinuo = !reproducirContinuo;
    }

    //Funciona para mostrar en un elemento "Text" la letra que se esta animando por el momento
    public void MostrarNombreLetraActual(char letra)
    {
        if (mostrarNombreAnimacion is null)
            return;
        mostrarNombreAnimacion.text = "Deletreando: " + letra;
    }

    //Funciona para quitar el nombre actual de la animacion en el elemento "Text"
    public void LimpiarNombreActual()
    {
        if (mostrarNombreAnimacion is null)
            return;
        mostrarNombreAnimacion.text = "-";
    }

    //Evento Start que pone el mismo unity
    private void Start()
    {
        estadoPersonaje = new EstadoEnEspera(this);
    }

    //Evento Update que pone unity
    private void Update()
    {
        if (animador == null)
            return;
        estadoPersonaje = estadoPersonaje.handleInput();
        if (Input.GetKeyDown(KeyCode.Alpha9))
            VerLog();
    }

    //mueve el puntero a mas uno para que el metodo de "ObtenerPalabraActual" obtengas la siguiente palabra en la lista
    private void SiguientePalabra()
    {
        puntero++;
    }

    //Obtiene la palabra actual del arreglo de palabras dependiendo de a donde apunta el valor del "puntero"
    private string ObtenerPalabraActual()
    {
        return palabrasEncontrados[puntero];
    }

    //Indica si el puntero llego o sobresalio a la canridad de palabras en el arreglo
    private bool LimiteListado()
    {
        return palabrasEncontrados.Length <= puntero;
    }

    //revisa si la palabra actual esta en el dicionario
    //No esta implementado aun asi que regresa -1
    private int EnDiccionario()
    {
        return -1;
    }

    //Funciona para ver que informacion esta en la lista y que numero tiene el puntero
    private void VerLog()
    {
        Debug.Log("Listaso: " + palabrasEncontrados);
        Debug.Log("puntero: " + puntero);
    }

    //regresa el puntero a cero para que comienze de nuevo las animaciones
    private void ResetListado()
    {
        puntero = 0;
        reproducirContinuo = false;
    }

    /* Clase: EstadoPersonaje
     * esta clase funciona para el metodo de update de unity, 
     * basicamente esta clase indica que estrucciones deve esperar
     * dependiendo del estado que se encuentre nuestro personaje que se anima
     * en escena.
     * este es la clase padre de los demas estados
     */
    class EstadoPersonje
    {
        //la isntancia del objeto reproductor, nesesario para poder acceder al animador
        protected ReproductorAnimacion instancia;

        public EstadoPersonje() { }
        public EstadoPersonje(ReproductorAnimacion setIstancia)
        {
            instancia = setIstancia;
        }

        //funcion heredada que todo los hijos deven tener, el lo que controla los input o entradas del programa, como los botones acciones y eso
        virtual public EstadoPersonje handleInput()
        {
            return this;
        }
    }

    /* Clase: EstadoEnEspera
     * este es el estado prinsipal el cual espera a la accion de reproducir las palabras,
     * si hay palabras en el arreglo de palabras (string) empesara a pasar de estado a realizar alguna animacion
     * si no existe una palabra para interactuar simplemente se regresa a si mismo
     */
    class EstadoEnEspera : EstadoPersonje
    {
        
        public EstadoEnEspera(ReproductorAnimacion setIstancia) : base(setIstancia) 
        {
            if (instancia.LimiteListado())
            {
                instancia.ResetListado();
            }
        }

        /* En este estado solo requerimos que el valor de reproductor Continuo este en true
         * tambien reproduc cundo la tecla "0" es presionado.
         */
        public override EstadoPersonje handleInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0) || instancia.reproducirContinuo) 
            {
                return ReproducirPalabra();
            }
            return this;
        }

        public EstadoPersonje ReproducirPalabra()
        {
            //revisa si no es la ultima palabra
            if (instancia.LimiteListado())
            {
                instancia.ResetListado();
                return new EstadoEnEspera(instancia);
            }

            //consige la palabra actual que esta apuntando la clase de instancia
            string actual = instancia.ObtenerPalabraActual();
            //revisa si lo tenemos definido en el diccionario (diccionario no implementado)
            int idpalabra = -1;
            idpalabra = instancia.EnDiccionario();
            if (idpalabra > -1)
                return new EstadoEjecutandoAnimacion(instancia, actual, idpalabra);
            //caso contrario deletreamos la palabra
            return new EstadoDeletreando(instancia, actual);
        }
    }

    /* Clase: EstadoDeletreando
     * este estado se entra al momento que la clase de "espera" decida ue la palabra deva de ser deletreada
     * en esta ocasion al crearse se guarda la palabra que se quiera deletrear, se incia el puntero a cero
     * y empieza a comodar la animacion de deletreo al personaje.
     * por lo general espera a que la animacion termine de ocurrir para luego activar lasiguiente palabra
     * (con un peque�o retraso definido en el objeto ReproductorAnimacion)
     * el retraso se aplica despues de animar.
     * una vez que se acaba la animacion y el retraso se aumenta el puntero para la siguiente letra
     * si el puntero llega al final se regresa al estado de espera
     */
    class EstadoDeletreando : EstadoPersonje
    {
        private string palabra;
        private char letraActual;
        private int puntero;
        private int milisegundos;
        private int retraso;
        public EstadoDeletreando(ReproductorAnimacion setIstancia,string palabraEnv) : base(setIstancia) 
        {
            palabra = palabraEnv;
            puntero = 0;
            milisegundos = instancia.miliSegundos;
            retraso = 0;
            instancia.animador.SetInteger("NumeroLetra", 0);
            instancia.animador.SetBool("Deletreo", true);
        }
        public override EstadoPersonje handleInput()
        {
            //Debug.Log("entro en Deletreo");
            float estado = instancia.animador.GetCurrentAnimatorStateInfo(0).normalizedTime;
            bool enTrancicion = instancia.animador.IsInTransition(0);
            //Debug.Log("current animation state: " + estado);
            //Debug.Log("retraso ms: " + retraso);
            if (estado > 1 && retraso < 1 && (!enTrancicion)) // revisa si sige animando
            {
                if (FinPalabra())
                {
                    instancia.animador.SetInteger("NumeroLetra", 0);
                    instancia.animador.SetBool("Deletreo", false);
                    instancia.SiguientePalabra();
                    return new EstadoEnEspera(instancia);
                }
                Proceso();
            }
            //Debug.Log("delta time: " + Time.deltaTime);
            if(estado > 1  && (retraso > 0))
            {
                //Debug.Log("entro al baja retraso");
                retraso = (int)(retraso - (Time.deltaTime * 1_000));
            }
             
            return this;
        }
        public int ObtenerNumeroLetra(char letra)
        {
            int respuesta = 0;
            if (letra == '�' || letra == '�')
                respuesta = 27;
            else if (letra >= 'a' && letra <= 'z')
                respuesta = letra - 'a' + 1;
            else if (letra >= 'A' && letra >= 'Z')
                respuesta = letra - 'A' + 1;
            return respuesta;
        } 

        public void SiguienteLetra()
        {
            puntero++;
        }

        public void ObtenerLetra()
        {
            letraActual = palabra[puntero];
        }

        public bool FinPalabra()
        {
            return palabra.Length <= puntero;
        }

        public void MostrarLetra (int numero)
        {
            instancia.animador.SetInteger("NumeroLetra", numero);
            retraso = milisegundos;
            //instancia.Invoke("QuirarNumLetra", milisegundos);
        }

        public void Proceso()
        {
            ObtenerLetra();
            instancia.MostrarNombreLetraActual(letraActual);
            MostrarLetra(ObtenerNumeroLetra(letraActual));
            SiguienteLetra();
        }
    }

    /* Clase: EstadoEjecutandoAnimacion
     * Este estado se entra al momento en que la clase de "espera" decida que la palabra deva ser animada
     * en una accion directa que tengamos registrada en el programa.
     * en este punto del dia 22 Mar no se a implementado ninguna palabra completa, pero si fuese el caso
     * reciviria la palabra en "string" y de ser el caso el numero de la animacion que deva reproducir 
     * el animador.
     * tendria que checar si se est animando, o si se esta reproduciendo y al acabarse regresar al estado de
     * espera, moviendo el puntero de la siguiente palabra.
     */
    class EstadoEjecutandoAnimacion : EstadoPersonje
    {
        private string palabra;
        private int idAnimacion;
        public EstadoEjecutandoAnimacion(ReproductorAnimacion setIstancia) : base(setIstancia)
        {
            this.palabra = "nada";
            this.idAnimacion = 0;
        }
        public EstadoEjecutandoAnimacion(ReproductorAnimacion setIstancia, string palabra, int numero) : base(setIstancia)
        {
            this.palabra = palabra;
            this.idAnimacion = numero;
        }
        public override EstadoPersonje handleInput()
        {

            return this;
        }
    }
}
