using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* script que se encarga de manejar las opciones que aparese en otras pantallas
* esta sujeto a cambios
*/

public class MenuOpciones : MonoBehaviour
{
    [SerializeField] private GameObject panelOpciones;

    public void OnMostrarPantallaOpciones()
    {
        MostrarPantallaOpciones();
    }

    public void OnSalirPrograma()
    {
        this.SalirPrograma();
    }

    private void MostrarPantallaOpciones()
    {
        if (panelOpciones == null)
            return;
        panelOpciones.SetActive(!(panelOpciones.activeSelf));
    }

    private void SalirPrograma()
    {
        Debug.Log("Cerrando programa");
        
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    void Start()
    {

        if (panelOpciones == null)
            panelOpciones = GameObject.Find("PanelMenu");

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            MostrarPantallaOpciones();
    }
}
