using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    codigo que simplemente funciona para mover una ventana sobrepuesta de las pantalla de traductor
    si no esta devinidas el CanvasRenderer de la ventana de animacion inicial no hay problema, solo recuerden que si
    es que lo tenemos y no esta asignada en esto, toda la pantalla estara bloqueada por la ventana inicial.
*/
public class PosicionarVentanaVIsta : MonoBehaviour
{
    [SerializeField] private CanvasRenderer ventanaVista;
    

    public void AjustarVentana()
    {
        if(ventanaVista != null)
            ventanaVista.GetComponent<Animator>().enabled = true;
    }
}
