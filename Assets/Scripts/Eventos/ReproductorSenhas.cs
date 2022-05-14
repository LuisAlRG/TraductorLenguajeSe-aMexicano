using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReproductorSenhas : MonoBehaviour
{
    /* Estos valores son los que se definen en la pantalla de edicion de uniti 
     * de los cuales son componentes
     * ----------------------------------------------------------------------------
     * Variable             Objetivo
     * suAnimador           Es el controlados de animador del personaje 3D 
     *                          definido por unity
     * salidaNombreAnimador Es un cuadro de texto (TextMeshPro) el cual muestra 
     *                          el nmbre de la animacion en ejecucion
     * listadoMostrable     Esto muestra las palabras encontrada en una lista 
     *                          horizontal encontrado en la pantalla del programa, 
     *                          se usa esta variable para hacer cambios a su aspecto
     * botonesReproduccion  Este elemento hace referencia al componente en pantalla 
     *                          que muestra los botones de reproduccion, se usa la 
     *                          variable para hacer cambio de aspecto
     */
    [SerializeField] private Animator suAnimador;
    [SerializeField] private TMP_Text salidaNombreAnimador;
    [SerializeField] private ListaANodo listadoMostrable;
    [SerializeField] private GrupoBotonReproductor botonesReproduccion;

    //esta variable sirbe para definir cuantos mili segundos hay entre letras al momento de deletrear.
    public int miliSegundoEspera;

    /* Estos valoresson de caracter privado, solo se modifican en su mismo codigo, 
     * por la razon de que requiero que sus modificaciones esten dado en funciones 
     * para poder ser llamadas en otros objetos, para asegurar que no esten vacias y eso.
     * ------------------------------------------------------------------------------
     * Variable             Objetivo
     * suEstado             objeto que define en que estado esta el personaje, importante
     *                          para definir que hacien hacer en determinado tiempo.
     * palabrasEncontrados  objeto que define el arreglo (o listado si les parese bien) 
     *                          de las palabras encontrados en el analizador de Palabras
     *                          tiene 2 campos, el texto de la palabra y su tipo.
     * puntero              valor que define donde se apunta en el arreglo de palabras
     *                          util para savr en que palabra estamos.
     * continuoReproduccion valor booleano que nos indica si la reproduccion las palabras 
     *                          accion es continua
     *                      -este tiene un "get" el cual permite consultar su valor 
     *                          publicamente, se llama "continuo"
     * veccesReproducir     valor que indica que tantas veses se quiere reporudcir un 
     *                          objeto en especifico (aoun en consideracion)
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
        salidaNombreAnimador.text = palabra+" / letra: " + letra;
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
    void Start()
    {
        if (suAnimador == null)
            suAnimador = GameObject.Find("Modelo_preparado").GetComponent<Animator>();
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
