using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/*
* Script que se encarga de mostrar el elemento palabra del programa
* para ser visible en la pantalla y tener algunas funciones con la 
* interaccion con el raton de la computadora.
* Tiene una referencia del elemento que engrupa todo los Nodos.
*/

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Outline))]
public class NodoPalabra : MonoBehaviour, IPointerClickHandler
{
    private ListaANodo listaPadre;
    [SerializeReference] private Image backGround;
    [SerializeReference] private TMP_Text textValue;
    [SerializeReference] private Outline contorno;
    private int tipo = 0;
    [SerializeReference] private int listIndex = 0;
    [SerializeReference] private string value = "-";
    [SerializeReference] private bool selected = false;
    private bool modificado = false;
    private Color[] coloresTipoBG;
    private Color[] coloresTipoText;


    public void SetColors(Color[] coloresFondo, Color[] colorTexto)
    {
        coloresTipoBG = coloresFondo;
        coloresTipoText = colorTexto;
        modificado = true;
    }
    public ListaANodo Padre
    {
        set { listaPadre = value; }
    }
    public int Tipo
    {
        set { tipo = value; modificado = true; }
    }
    public int Puntero
    {
        get { return listIndex; }
        set { listIndex = value; }
    }
    public string ValueText
    {
        get { return value; }
        set
        {
            this.value = value.ToString();
            if (textValue != null)
                textValue.text = value;
            
        }
    }
    
    public void ToggleSelected()
    {
        selected = !selected;
        modificado = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        listaPadre.OnSeleccionado(this);
    }

    public void Coloring()
    {
        if (backGround == null)
            return;
        if (textValue == null)
            return;
        int colorIndx = tipo;
        if (colorIndx >= coloresTipoBG.Length)
            colorIndx = 0;
        //Debug.Log("si Entra a ver si cambia color");
        //Debug.Log(coloresTipoBG[colorIndx]);
        //Debug.Log(coloresTipoText[colorIndx]);
        backGround.color = coloresTipoBG[colorIndx];
        textValue.color = coloresTipoText[colorIndx];

        if (contorno == null)
            return;
        contorno.enabled = selected;

        modificado = false;
    }
    void Start()
    {
        backGround = GetComponent<Image>();
        textValue = GetComponentInChildren<TMP_Text>();
        contorno = GetComponent<Outline>();
        if (coloresTipoBG == null)
            coloresTipoBG = new Color[] { backGround.color };
        if (coloresTipoText == null)
        {
            coloresTipoText = new Color[] { textValue.color };
        }
        if (textValue != null)
            textValue.text = value;
        Coloring();
        
    }

    void Update()
    {
        if (modificado)
        {
            Coloring();
            
        }
    }

    public void ResetToColor()
    {
        if (backGround == null)
            return;
        backGround.color = coloresTipoBG[0];
        if (textValue == null)
            return;
        textValue.color = coloresTipoText[0];
    }

    public void Seleccionado()
    {
        ToggleSelected();
        
    }
    public void DeSeleccionado()
    {
        ToggleSelected();
    }
}
