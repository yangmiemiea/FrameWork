using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class EventTable : BaseBehaviour {

    [SerializeField]
    public UIEvent[] events;

    Dictionary<string, UIEvent> eventDic = new Dictionary<string, UIEvent>();

    protected override void Awake()
    {      
        foreach (var item in events)
        {
            item.ClickEventHandler = null;
            eventDic.Add(item.eventName, item);
        }
    }

    public void ListenEvent(string eventName, UnityAction call)
    {
        if (eventDic.ContainsKey(eventName))
        {
            if (eventDic[eventName].ClickEventHandler != null)
            {
                eventDic[eventName].ClickEventHandler += call;
            }
            else
            {
                eventDic[eventName].ClickEventHandler = call;
            }
        }
        else
        {
            Debug.LogError("no such event,check it out!");
        }
    }

    public UnityAction GetUnityAction(string eventName)
    {
        return eventDic[eventName].ClickEventHandler;
    }

    public void ClearEvent(string eventName)
    {
        if (eventDic.ContainsKey(eventName))
        {
            eventDic.Remove(eventName);
        }
    }

    public void ClearAllEvent()
    {
        eventDic.Clear();
    }

    protected override void OnDestroy()
    {
        foreach (var item in events)
        {
            item.ClickEventHandler = null;
        }
    }
}

[Serializable]
public class UIEvent
{
    public string eventName;

    public GameObject btnObj;

    [HideInInspector]
    public UnityAction ClickEventHandler;
}