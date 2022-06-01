using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
* Script que se encarga de manejar el animador del personaje en pantalla
* básicamente se encarga de mantener el estado del personaje y dependiendo de
* la accion del usuario en los botones de reproducción enviá las señales de 
* reproducir las animaciones y manejar el objeto "Animator" para realizar la
* acción correspondiente
* Por decirlo así, es el encargado de comunicarse el estado del personaje con lo
* que se ve en pantalla.
*/

public class ReproductorSenhas : MonoBehaviour
{
    /* Estos valores son los que se definen en la pantalla de edición de Unity 
     * de los cuales son componentes
     * ----------------------------------------------------------------------------
     * Variable             Objetivo
     * suAnimador           Es el controlados de animador del personaje 3D 
     *                          definido por Unity
     * salidaNombreAnimador Es un cuadro de texto (TextMeshPro) el cual muestra 
     *                          el nombre de la animación en ejecución
     * listadoMostrable     Esto muestra las palabras encontrada en una lista 
     *                          horizontal encontrado en la pantalla del programa, 
     *                          se usa esta variable para hacer cambios a su aspecto
     * botonesReproduccion  Este elemento hace referencia al componente en pantalla 
     *                          que muestra los botones de reproducción, se usa la 
     *                          variable para hacer cambio de aspecto
     */
    [SerializeField] private Animator suAnimador;
    [SerializeField] private TMP_Text salidaNombreAnimador;
    [SerializeField] private ListaANodo listadoMostrable;
    [SerializeField] private GrupoBotonReproductor botonesReproduccion;

    //esta variable sirbe para definir cuantos mili segundos hay entre letras al momento de deletrear.
    public int miliSegundoEspera;

    /* Estos valores son de carácter privado, solo se modifican en su mismo código, 
     * por la razón de que requiero que sus modificaciones estén dado en funciones 
     * para poder ser llamadas en otros objetos, para asegurar que no esten vacias y eso.
     * ------------------------------------------------------------------------------
     * Variable             Objetivo
     * suEstado             objeto que define en que estado esta el personaje, importante
     *                          para definir que hacer en determinado tiempo.
     * palabrasEncontrados  objeto que define el arreglo (o listado si les parece bien) 
     *                          de las palabras encontrados en el analizador de Palabras
     *                          tiene 2 campos, el texto de la palabra y su tipo.
     * puntero              valor que define donde se apunta en el arreglo de palabras
     *                          útil para saber en que palabra estamos.
     * continuoReproduccion valor booleano que nos indica si la reproducción las palabras 
     *                          acción es continua
     *                      -este tiene un "get" el cual permite consultar su valor 
     *                          públicamente, se llama "continuo"
     * veccesReproducir     valor que indica que tantas beses se quiere reproducir un 
     *                          objeto en especifico (aun en consideración)
     */
    private EstadoAtualPersonaje suEstado;
    private Palabra[] palabrasEncontrados;
    private int puntero;
    private bool continuoReproduccion;
    public bool continuo
    {
        get { return continuoReproduccion; }
    } 
    private int vecesRepoducir;
    private bool interrumpir = false;

    public void PrepararParaDeletreo()
    {
        if (suAnimador == null)
            return;
        suAnimador.SetInteger("NumeroLetra", 0);
        suAnimador.SetBool("Deletreo", true);
    }

    public void MostrarLetraDeletreo(int numero)
    {
        suAnimador.SetInteger("NumeroLetra", numero);
    }

    public void MostrarAnimacionPalabra(int numero){
        suAnimador.SetInteger("NumeroPalabra", numero);
    }

    public void DetenerDeletreo()
    {
        if (suAnimador == null)
            return;
        suAnimador.SetInteger("NumeroLetra", 0);
        suAnimador.SetBool("Deletreo", false);
    }

    public float EstaEnAnimacion()
    {
        if(suAnimador == null)
            return 1.1f;
        return suAnimador.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public bool EnTrancicion()
    {
        if (suAnimador == null)
            return false;
        return suAnimador.IsInTransition(0);
    }

    public void RecivirPalabrasEncontradas(string[] textos, int[] tipos)
    {
        if (textos == null)
            return;
        if (textos.Length == 0)
            return;
        palabrasEncontrados = new Palabra[textos.Length];
        for(int i = 0; i < textos.Length; i++)
        {
            string texto = textos[i];
            int tipo = 0;
            if (i > tipos.Length)
                tipo = tipos[i];
            palabrasEncontrados[i] = new Palabra(texto,tipo);
        }
        suEstado = new EstadoEnEspera(this);
        puntero = 0;
    }

    public void CambiarReproduccionCotinuo()
    {
        continuoReproduccion = !continuoReproduccion;
    }

    public void IniciarReproduccionContinuo()
    {
        continuoReproduccion = true;
    }

    public void FinalizarReproduccionContinuo()
    {
        continuoReproduccion = false;
    }

    public void AderirVecesRerpoducido()
    {
        vecesRepoducir++;
    }

    public void AderirVecesRerpoducido(int veces)
    {
        vecesRepoducir = veces;
        continuoReproduccion = false;
    }

    public bool ReproducirVeces()
    {
        if (vecesRepoducir > 0)
        {
            vecesRepoducir--;
            return true;
        }
        return false;
    }

    public void MostrarLetraActual(char letra)
    {
        if (salidaNombreAnimador == null)
            return;
        salidaNombreAnimador.text = "Deletreado: " + letra;
    }

    public void MostrarLetraActual(string palabra, char letra)
    {
        if (salidaNombreAnimador == null)
            return;
        //salidaNombreAnimador.text = palabra+" / letra: " + letra;
        salidaNombreAnimador.text = "Letra: " + letra + " / " + palabra;
    }

    public void LimpiarNombreActualAnimacion()
    {
        if (salidaNombreAnimador == null)
            return;
        salidaNombreAnimador.text = "Name Space";
    }

    public void MoverSiguientePalabra()
    {
        puntero++;
        if (palabrasEncontrados != null)
            if (puntero >= palabrasEncontrados.Length)
            {
                this.ResetearLista();
                puntero = palabrasEncontrados.Length - 1;
            }
            else { }
        else
        {
            this.ResetearLista();
        }
        listadoMostrable.MoverSiguientePalabra();
    }

    public void MoverAtrasPalabra()
    {
        puntero--;
        if (puntero < 0)
            this.ResetearLista();
        listadoMostrable.MoverAnteriorPalabra();
    }

    public void SetPunteroConIndex(int numero)
    {
        puntero = numero;
        if (palabrasEncontrados != null)
            if (puntero >= palabrasEncontrados.Length)
            {
                this.ResetearLista();
                puntero = palabrasEncontrados.Length - 1;
            }
            else { }
        else if (puntero < 0)
        {
            this.ResetearLista();
        }
    }

    public void ResetearLista()
    {
        puntero = 0;
        continuoReproduccion = false;
        vecesRepoducir = 0;
        botonesReproduccion.DeseleccionarBotonSeleccionado();
    }

    public Palabra ObtenerPalabraActual()
    {
        return palabrasEncontrados[puntero];
    }

    public void AplicarInterrupcion()
    {
        interrumpir = true;
        botonesReproduccion.DeseleccionarBotonSeleccionado();
    }

    public void RecargarMilisegundosEspera()
    {
        if (MainManager.ManagerInstancia == null)
            return;
        int checarRetraso = MainManager.ManagerInstancia.MiliSegundoEsperaGlobal;
        //Debug.Log("elRetrasoEsDe: " + checarRetraso);
        if (checarRetraso > 400)
            miliSegundoEspera = checarRetraso;
    }

    void Start()
    {
        if (suAnimador == null)
            suAnimador = GameObject.Find("Modelo_preparado").GetComponent<Animator>();
        if (MainManager.ManagerInstancia == null)
            return;
        int checarRetraso = MainManager.ManagerInstancia.MiliSegundoEsperaGlobal;
        if (checarRetraso > 400)
            miliSegundoEspera = checarRetraso;
    }

    // Update is called once per frame
    void Update()
    {
        if (palabrasEncontrados == null)
            return;
        if (palabrasEncontrados.Length == 0)
            return;
        if (suAnimador == null)
            return;
        if (suEstado == null)
            suEstado = new EstadoEnEspera(this);
        suEstado = suEstado.handleInput();

        if (interrumpir)
        {
            DetenerDeletreo();
            LimpiarNombreActualAnimacion();
            vecesRepoducir = 0;
            continuoReproduccion = false;
            suEstado = new EstadoEnEspera(this);
            interrumpir = false;
        }
    }
}
