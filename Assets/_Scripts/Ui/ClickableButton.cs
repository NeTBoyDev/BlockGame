using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableButton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
    public event Action<PointerEventData> OnPointerDown;
    public event Action<PointerEventData> OnPointerExit;
    
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnPointerDown?.Invoke(eventData);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        OnPointerExit?.Invoke(eventData);
    }
}
