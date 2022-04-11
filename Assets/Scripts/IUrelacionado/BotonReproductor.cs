using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class BotonReproductor : MonoBehaviour, 
    IPointerEnterHandler, 
    IPointerClickHandler,
    IPointerDownHandler, 
    IPointerUpHandler,
    IPointerExitHandler
{
    [SerializeField] private GrupoBotonReproductor grupoBotones;
    public Image background;
    public bool sostenible = false;
    public UnityEvent onBotonSelected;
    public UnityEvent onBotonDeSelected;

    public void OnPointerClick(PointerEventData eventData)
    {
        grupoBotones.OnBotonSelected(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        grupoBotones.OnBotonDown(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        grupoBotones.OnBotonUp(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        grupoBotones.OnBotonEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        grupoBotones.OnBotonExit(this);
    }

    void Start()
    {
        background = GetComponent<Image>();
        if(grupoBotones == null)
            grupoBotones = GetComponentInParent<GrupoBotonReproductor>();
        grupoBotones.Subscribirte(this);
    }

    public void Select()
    {
        if (onBotonSelected != null)
        {
            onBotonSelected.Invoke();
        }
    }

    public void DeSelect()
    {
        if (onBotonDeSelected != null)
        {
            onBotonDeSelected.Invoke();
        }
    }

    
}
