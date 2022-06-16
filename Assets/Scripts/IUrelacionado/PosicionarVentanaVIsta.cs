using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionarVentanaVIsta : MonoBehaviour
{
    [SerializeField] private CanvasRenderer ventanaVista;
    

    public void AjustarVentana()
    {
        if(ventanaVista != null)
            ventanaVista.GetComponent<Animator>().enabled = true;
    }
}
