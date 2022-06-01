using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

/*
* script que se encarga de manejar las opciones que aparece 
* en la pantalla principal del programa.
* Esta sujeto a cambios
*/



public class MenuPrincipalOpciones : MonoBehaviour
{
    /* Valores que se definen en la pantalla de edición
     * de Unity de los cuales son componentes
     * ---
     * Variables                Objetivo
     * audioMixer               Este es una referencia para la música o 
                                sonido de fondo, no hay ninguno por 
                                ahora pero por si acaso esta el método 
                                de subir volumen.
     * opcionesDeResoluciones   Elemento "drop down" que tiene las resoluciones
                                lo requerimos para poder sacar las posibles
                                resoluciones del programa en la computadora.
     * velocidadAnimacionText   Texto que muestra el valor de segundos de la
                                variable velocidad de retraso.
     * barraVelocidadAnimacion  Barra que funciona para darle valor a la variable
                                velocidad de retraso.
     * checkBoxPantallaCompleta Variable que se usa para cambiar el estado de check
                                de la pantalla completa.
     * resoluciones             Esta variable guarda la posibles resoluciones y puede
                                usarse para decirle que resolución queremos.
     *
     *
     */
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown opcionesDeResoluciones;
    [SerializeField] private TMP_Text velocidadAnimacionText;
    [SerializeField] private Scrollbar barraVelocidadAnimacion;
    [SerializeField] private Toggle checkBoxPantallaCompleta;
    
    // Esta variable guarda la posibles resoluciones y 
    // usarse para decirle que resolucion queremos.
    private Resolution[] resoluciones;
    private int calidadIndex = 0;
    private bool quiereFullScreen = false;
    private int resolucionIndex = 0;

    private bool opcionesModificada = false; 

    public void Start()
    {
        //Parte de resolucion de pantalla
        resoluciones = Screen.resolutions;
        recargarResoluciones();
        ReloadVelosidadAnimacionText();
        RevisarPantallaCompleta();

        RevisarCalidad();
    }

    public void recargarResoluciones()
    {
        int resolucionConcurrenteIndex = 0;
        if (opcionesDeResoluciones == null)
            return;
        opcionesDeResoluciones.ClearOptions();
        List<string> textoDeReolocuines = new List<string>();
        Resolution anteriorResolucion = new Resolution();
        anteriorResolucion.width = 0;
        anteriorResolucion.height = 0;
        for (int i = 0; i < resoluciones.Length; i++)
        {
            Resolution res = resoluciones[i];
            string opcionInd = res.width + " X " + res.height + " " + res.refreshRate +"Hz";
            textoDeReolocuines.Add(opcionInd);
            if (res.width == Screen.width &&
                res.height == Screen.height &&
                res.refreshRate == Screen.currentResolution.refreshRate
                )
                resolucionConcurrenteIndex = i;
        }
        //Debug.Log("En resolucion: "+ Screen.currentResolution.width + "X" + Screen.currentResolution.height);
        Debug.Log(Screen.currentResolution);
        //Debug.Log("Index de resolucion: " + resolucionConcurrenteIndex);
        opcionesDeResoluciones.AddOptions(textoDeReolocuines);
        opcionesDeResoluciones.value = resolucionConcurrenteIndex;
        opcionesDeResoluciones.RefreshShownValue();
    }

    public float MapearNumero(float numero, float a1, float a2, float b1, float b2)
    {
        float difA = (a2 - a1);
        if (difA >= 0.001f && difA <= 0f)
            return b1;
        return b1 + (numero - a1) * (b2 - b1) / (a2 - a1);
    }
    public void OnAbrirTraductor()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void OnAbrirTraductorArchivo()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void OnAplicarOpciones()
    {
        if (opcionesModificada)
        {
            Resolution elResolucion = resoluciones[resolucionIndex];
            QualitySettings.SetQualityLevel(calidadIndex);
            //Debug.Log(elResolucion);
            Screen.SetResolution(elResolucion.width, elResolucion.height, quiereFullScreen, elResolucion.refreshRate);
            //Debug.Log(Screen.currentResolution);

            //recargarResoluciones();
        }
    }

    public void OnSalir()
    {
        Debug.Log("Cerrando programa");
        Application.Quit();
    }

    public void OnCambiarVolumen(float volume)
    {
        Debug.Log(volume);
        float correcto = MapearNumero(volume,0f,1f,-80f,-0.01f);
        
        correcto = 20f * Mathf.Log10(correcto);
        Debug.Log(correcto);
        audioMixer.SetFloat("VolumenMaster", correcto);
    }

    public void OnSetQuality (int index)
    {
        //QualitySettings.SetQualityLevel(Index);
        calidadIndex = index;
        opcionesModificada = true;
    }

    public void OnFullScreen(bool loQuiere)
    {
        //Screen.fullScreen = loQuiere;
        quiereFullScreen = loQuiere;
        opcionesModificada = true;
    }

    public void OnCambiarTamanhoPantalla(int index)
    {
        //Resolution elResolucion = resoluciones[index];
        //Screen.SetResolution(elResolucion.width, elResolucion.height, Screen.fullScreen);
        resolucionIndex = index;
        opcionesModificada = true;
    }

    public void OnCambiarVelocidadAnimacion(float valor)
    {
        float correcto = MapearNumero(valor, 0, 1, 500, 5_000);
        SetVelosidadAnimacionText(correcto);
        if (MainManager.ManagerInstancia != null)
            MainManager.ManagerInstancia.MiliSegundoEsperaGlobal = (int)correcto;
        PlayerPrefs.SetInt("MiliSegundoEsperaGlobal", (int)correcto );
    }

    public void SetVelosidadAnimacionText(float valor)
    {
        string valorAMostrar = (valor / 1000).ToString("F2");
        if (velocidadAnimacionText != null)
        {
            velocidadAnimacionText.text = valorAMostrar + "S";
        }
    }

    public void ReloadVelosidadAnimacionText()
    {
        int segundosChecar = PlayerPrefs.GetInt("MiliSegundoEsperaGlobal");
        if (segundosChecar < 50)
            return;
        if (MainManager.ManagerInstancia == null)
            return;
        MainManager.ManagerInstancia.MiliSegundoEsperaGlobal = segundosChecar;
        float correcto = MapearNumero(segundosChecar, 500, 5_000, 0, 1);
        if (barraVelocidadAnimacion == null)
            return;
        barraVelocidadAnimacion.value = correcto;
        SetVelosidadAnimacionText(segundosChecar);
    }

    public void RevisarPantallaCompleta()
    {
        if (checkBoxPantallaCompleta == null)
            return;
        checkBoxPantallaCompleta.isOn = Screen.fullScreen;
    }

    public void RevisarCalidad()
    {

    }
}
