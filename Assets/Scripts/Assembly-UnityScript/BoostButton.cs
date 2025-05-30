using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoostButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GuiControl control;

    public void OnPointerDown(PointerEventData eventData)
    {
        control.boostFound = true;
       // Debug.Log("Hi");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        control.boostFound = false;
        //Debug.Log("Bye");
    }
}
