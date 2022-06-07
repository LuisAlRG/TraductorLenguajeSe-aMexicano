using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Script que se encarga de manejar una lista de "NodoPalabra" que
* permite guardar cada palabra como un nodo y mostrarlo en un Elemento
* de pantalla.
* También se encarga un poco del aspecto de los Nodos (o capsulas) que 
* aparece en pantalla con algunas funciones de interacción con la lista
* (como seria el avanzar de palabra cambiando su indice o interactuar 
* con los botones de reproducción).
*/

public class ListaANodo : MonoBehaviour
{
    [SerializeField] private string[] palabrasEncontrados;

    public GameObject NodoPrefap;
    public Color colorDeBase;
    public Color colorConocido;
    public Color colorDesconocido;

    [SerializeField] private List<NodoPalabra> listaNodos;
    [SerializeField] private NodoPalabra nodoSeleccionado;

    [SerializeField] private ReproductorSenhas elReproductor;

    public void AderirNodos (string[] palabras, int[] tipo)
    {
        palabrasEncontrados = palabras;
        if (nodoSeleccionado != null)
            nodoSeleccionado = null;
        if(listaNodos != null)
            listaNodos.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i< palabras.Length; i++)
        {
            int queTipoEs = 1;
            if(i<tipo.Length)
                queTipoEs = tipo[i];
            NodoPalabra modificable = NodoPrefap.GetComponent<NodoPalabra>();
            modificable.ValueText = palabras[i];
            GameObject nuevoObjeto =  Instantiate(NodoPrefap, transform);
            NodoPalabra suNodo = nuevoObjeto.GetComponent<NodoPalabra>();
            suNodo.Padre = this;
            //suNodo.ValueText = palabras[i];
            suNodo.Tipo = queTipoEs;
            suNodo.Puntero = i;
            if (i == 0)
            {
                suNodo.ToggleSelected();
                nodoSeleccionado = suNodo;
            }
            suNodo.SetColors(
                new Color[] { colorDeBase, colorConocido, colorDesconocido },
                new Color[] {Color.black, Color.white, Color.white}
                );
            listaNodos.Add(suNodo);
            nuevoObjeto.transform.SetParent(gameObject.transform, false);
            //gameObject.transform.SetAsLastSibling();
        }
        Canvas.ForceUpdateCanvases();
    }

    public void AderirNodos (Palabra[] palabras){
        if (nodoSeleccionado != null)
            nodoSeleccionado = null;
        if(listaNodos != null)
            listaNodos.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        palabrasEncontrados = new string[palabras.Length];
        for(int i=0; palabras.Length > i ; i++){
            palabrasEncontrados[i] = palabras[i].texto;

            NodoPalabra modificable = NodoPrefap.GetComponent<NodoPalabra>();
            modificable.ValueText = palabras[i].texto;
            GameObject nuevoObjeto =  Instantiate(NodoPrefap, transform);
            NodoPalabra suNodo = nuevoObjeto.GetComponent<NodoPalabra>();
            suNodo.Padre = this;
            //suNodo.ValueText = palabras[i];
            suNodo.Tipo = palabras[i].tipo;
            suNodo.Puntero = i;
            if (i == 0)
            {
                suNodo.ToggleSelected();
                nodoSeleccionado = suNodo;
            }
            suNodo.SetColors(
                new Color[] { colorDeBase, colorConocido, colorDesconocido },
                new Color[] {Color.black, Color.white, Color.white}
                );
            listaNodos.Add(suNodo);
            nuevoObjeto.transform.SetParent(gameObject.transform, false);
        }
        Canvas.ForceUpdateCanvases();
    }
    public void MoverSiguientePalabra()
    {
        if (nodoSeleccionado == null)
            return;
        int index = nodoSeleccionado.Puntero;
        index++;
        if (index >= listaNodos.Count)
            return;
        nodoSeleccionado.DeSeleccionado();
        nodoSeleccionado = listaNodos[index];
        nodoSeleccionado.Seleccionado();
    }

    public void MoverAnteriorPalabra()
    {
        if (nodoSeleccionado == null)
            return;
        int index = nodoSeleccionado.Puntero;
        index--;
        if (index < 0)
            return;
        nodoSeleccionado.DeSeleccionado();
        nodoSeleccionado = listaNodos[index];
        nodoSeleccionado.Seleccionado();
    }
    void Start()
    {
        //AderirNodos(palabrasEncontrados, new int[] {1,2});
        if (elReproductor == null)
            elReproductor = GameObject.Find("ReproductorCompleto").GetComponent<ReproductorSenhas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void OnSeleccionado(NodoPalabra nodo)
    {
        if(nodoSeleccionado != null)
        {
            nodoSeleccionado.DeSeleccionado();
        }
        nodoSeleccionado = nodo;
        nodoSeleccionado.Seleccionado();
        int index = nodoSeleccionado.Puntero;
        elReproductor.SetPunteroConIndex(index);
    }
}
