using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrupoBotonReproductor : MonoBehaviour
{
    [SerializeReference] private List<BotonReproductor> losBotones;
    public Color botonIdle;
    public Color botonHover;
    public Color botonActive;
    [SerializeReference] private BotonReproductor selectedBoton;

    public void Subscribirte(BotonReproductor boton)
    {
        if (losBotones == null)
            losBotones = new List<BotonReproductor>();
        losBotones.Add(boton);
    }

    public void OnBotonEnter(BotonReproductor boton)
    {

        if(selectedBoton != null)
            if (selectedBoton == boton)
                return;
        boton.background.color = botonHover;
    }

    public void OnBotonExit(BotonReproductor boton)
    {
        if (selectedBoton != null)
            if (selectedBoton == boton)
                return;
        boton.background.color = botonIdle;
    }

    public void OnBotonSelected(BotonReproductor boton)
    {
        if (selectedBoton != null)
        {
            if (selectedBoton == boton)
                return;
            if (boton.sostenible)
            {
                selectedBoton.DeSelect();
                selectedBoton.background.color = botonIdle;
            }
        }
        if (boton.sostenible)
        {
            
            selectedBoton = boton;
            selectedBoton.background.color = botonActive;
        }
        boton.Select();
            
        //int index = selectedBoton.transform.GetSiblingIndex();
    }

    public void OnBotonDown(BotonReproductor boton)
    {
        boton.background.color = botonActive;
    }

    public void OnBotonUp(BotonReproductor boton)
    {
        if(boton.sostenible)
            boton.background.color = botonActive;
        else
            boton.background.color = botonIdle;
    }

    
    public void DeseleccionarBotonSeleccionado()
    {
        if (selectedBoton == null)
            return;
        selectedBoton.DeSelect();
        selectedBoton.background.color = botonIdle;
        selectedBoton = null;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
