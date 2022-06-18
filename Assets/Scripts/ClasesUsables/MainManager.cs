using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script que mantiene parámetros de uso global.
 */
public class MainManager : MonoBehaviour
{
    public static MainManager ManagerInstancia;
   
    //Datos que quiero pasar
    public int MiliSegundoEsperaGlobal = 800;
    public List<string>[] MegaDiccionario;  
    public bool SaludoPrincipalAnim = true;

    private void Awake()
    {
        if(ManagerInstancia != null)
        {
            Destroy(gameObject);
            return;
        }

        MiliSegundoEsperaGlobal = PlayerPrefs.GetInt("MiliSegundoEsperaGlobal");
        SaludoPrincipalAnim = PlayerPrefs.GetInt("SaludoPrincipalActive") == 1;
        ManagerInstancia = this;
        DontDestroyOnLoad(gameObject);
        MegaDiccionario = new List<string>[27]; 
        LeerDiccionarios();

    }

    private void LeerDiccionarios(){
        GuardarDiccionarioActual("LetraA",ObtenerNumeroLetra('a'));
        GuardarDiccionarioActual("LetraB",ObtenerNumeroLetra('b'));
        GuardarDiccionarioActual("LetraC",ObtenerNumeroLetra('c'));
        GuardarDiccionarioActual("LetraD",ObtenerNumeroLetra('d'));
        GuardarDiccionarioActual("LetraE",ObtenerNumeroLetra('e'));
        GuardarDiccionarioActual("LetraF",ObtenerNumeroLetra('f'));
        GuardarDiccionarioActual("LetraG",ObtenerNumeroLetra('g'));
        GuardarDiccionarioActual("LetraH",ObtenerNumeroLetra('h'));
        GuardarDiccionarioActual("LetraI",ObtenerNumeroLetra('i'));
        GuardarDiccionarioActual("LetraJ",ObtenerNumeroLetra('j'));
        GuardarDiccionarioActual("LetraK",ObtenerNumeroLetra('k'));
        GuardarDiccionarioActual("LetraL",ObtenerNumeroLetra('l'));
        GuardarDiccionarioActual("LetraM",ObtenerNumeroLetra('m'));
        GuardarDiccionarioActual("LetraN",ObtenerNumeroLetra('n'));
        GuardarDiccionarioActual("LetraÑ",ObtenerNumeroLetra('ñ'));
        GuardarDiccionarioActual("LetraO",ObtenerNumeroLetra('o'));
        GuardarDiccionarioActual("LetraP",ObtenerNumeroLetra('p'));
        GuardarDiccionarioActual("LetraQ",ObtenerNumeroLetra('q'));
        GuardarDiccionarioActual("LetraR",ObtenerNumeroLetra('r'));
        GuardarDiccionarioActual("LetraS",ObtenerNumeroLetra('s'));
        GuardarDiccionarioActual("LetraT",ObtenerNumeroLetra('t'));
        GuardarDiccionarioActual("LetraU",ObtenerNumeroLetra('u'));
        GuardarDiccionarioActual("LetraV",ObtenerNumeroLetra('v'));
        GuardarDiccionarioActual("LetraW",ObtenerNumeroLetra('w'));
        GuardarDiccionarioActual("LetraX",ObtenerNumeroLetra('x'));
        GuardarDiccionarioActual("LetraY",ObtenerNumeroLetra('y'));
        GuardarDiccionarioActual("LetraZ",ObtenerNumeroLetra('z'));
    }

    private void GuardarDiccionarioActual(string direccion, int index){
        TextAsset textFile = Resources.Load<TextAsset>("Diccionario/" + direccion);
        if(textFile == null)
            return;
        string[] unitDiccionario = textFile.text.Split('\n');
        int idGrupal = index * 1000;
        if(MegaDiccionario[index] == null)
            MegaDiccionario[index] = new List<string>();
        for(int i = 1; unitDiccionario.Length > i; i++){
            (MegaDiccionario[index]).Add(unitDiccionario[i].TrimEnd() + "|" + (idGrupal + i));
            //Debug.Log(unitDiccionario[i] + "|" + (idGrupal + i));
        }
        //Debug.Log(MegaDiccionario);
        //Debug.Log(MegaDiccionario.Length);
        //Debug.Log(MegaDiccionario[index].Count);
    }

    private int ObtenerNumeroLetra(char letra)
    {
        int respuesta = -1;
        if (letra == 'ñ' || letra == 'Ñ')
            respuesta = 14;
        if (letra >= 'a' && letra <= 'n')
            respuesta = letra - 'a';
        if (letra >= 'A' && letra <= 'N')
            respuesta = letra - 'A';
        if (letra >= 'p' && letra <= 'z')
            respuesta = letra - 'a' + 1;
        if (letra >= 'P' && letra <= 'Z')
            respuesta = letra - 'A' + 1;
        if (letra == 'á' || letra == 'Á')
            respuesta = 0;
        if (letra == 'é' || letra == 'É')
            respuesta = 4;
        if (letra == 'í' || letra == 'Í')
            respuesta = 8;
        if (letra == 'ó' || letra == 'Ó')
            respuesta = 15;
        if (letra == 'ú' || letra == 'Ú')
            respuesta = 21;
        return respuesta;
    }

    public List<string> ObtenerDiccionarioLetra(char letra){
        int numLetra = ObtenerNumeroLetra(letra);
        if(numLetra == -1)
            return null;
        return MegaDiccionario[numLetra];
    }

    public static void SalvarAnimacionSaludoInicial(int valor){
        PlayerPrefs.SetInt("SaludoPrincipalActive",valor);
    }
}
