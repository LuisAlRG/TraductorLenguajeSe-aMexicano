using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

/*
* script que se encarga de manejar las opciones que aparese en la pantalla principal del programa
* esta sujeto a cambios
*/

public class MenuPrincipalOpciones : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown opcionesDeResoluciones;
    private Resolution[] resoluciones;

    private int calidadIndex = 0;
    private bool quiereFullScreen = false;
    private int resolucionIndex = 0;

    private bool opcionesModificada = false; 

    public void Start()
    {
        //Parte de resolucion de pantalla
        resoluciones = Screen.resolutions;
        int resolucionConcurrenteIndex = 0;
        recargarResoluciones();
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
        Debug.Log("En resolucion: "+ Screen.currentResolution.width + "X" + Screen.currentResolution.height);
        Debug.Log(Screen.currentResolution);
        Debug.Log("Index de resolucion: " + resolucionConcurrenteIndex);
        opcionesDeResoluciones.AddOptions(textoDeReolocuines);
        opcionesDeResoluciones.value = resolucionConcurrenteIndex;
        opcionesDeResoluciones.RefreshShownValue();
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
            Debug.Log(elResolucion);
            Screen.SetResolution(elResolucion.width, elResolucion.height, quiereFullScreen, elResolucion.refreshRate);
            

            Debug.Log(Screen.currentResolution);

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
        
        audioMixer.SetFloat("VolumenMaster", volume);
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
}
