using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * script usado para desactivar la pantalla de Inicio de las secicones de Traductor 
 * por si el usuario lo quiere desactivado
 */

public class ScriptVentanaInicio : MonoBehaviour
{
    private void Awake()
    {
        if (MainManager.ManagerInstancia == null)
            return;
        DesactivarVentana(MainManager.ManagerInstancia.SaludoPrincipalAnim);
    }

    public void DesactivarVentana(bool loquiere)
    {
        gameObject.SetActive(loquiere);
    }
}
