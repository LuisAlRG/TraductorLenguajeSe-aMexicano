using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
* script que se encarga de manejar las opciones que aparece en otras pantallas
* esta sujeto a cambios
*/

public class MenuOpciones : MenuPrincipalOpciones
{
    [SerializeField] private GameObject panelMenu;

    public void OnMostrarPantallaOpciones()
    {
        MostrarPantallaOpciones();
    }

    private void MostrarPantallaOpciones()
    {
        if (panelMenu == null)
            return;
        panelMenu.SetActive(!(panelMenu.activeSelf));
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            MostrarPantallaOpciones();
    }

    public void OnRegresarInicio()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}