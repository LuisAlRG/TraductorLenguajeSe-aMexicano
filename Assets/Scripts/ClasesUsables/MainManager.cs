using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager ManagerInstancia;

    //Datos que quiero pasar
    public int MiliSegundoEsperaGlobal = 800;

    private void Awake()
    {
        if(ManagerInstancia != null)
        {
            Destroy(gameObject);
            return;
        }

        MiliSegundoEsperaGlobal = PlayerPrefs.GetInt("MiliSegundoEsperaGlobal");

        ManagerInstancia = this;
        DontDestroyOnLoad(gameObject);
    }

}
