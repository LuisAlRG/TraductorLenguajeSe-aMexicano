using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
* script que se encarga de manejar las opciones que aparese en la pantalla principal del programa
* esta sujeto a cambios
*/

public class MenuPrincipalOpciones : MonoBehaviour
{
    
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

    }

    public void OnSalir()
    {
        Debug.Log("Cerrando programa");
        Application.Quit();
    }
}
