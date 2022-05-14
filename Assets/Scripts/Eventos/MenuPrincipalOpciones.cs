using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
