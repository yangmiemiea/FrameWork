using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventBindClick :EventBind, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        UnityAction call = table.GetUnityAction(bindEvent.eventName);
        if (call != null)
        {
            call();
        }
    }
}
