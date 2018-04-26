using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableToValue {
    //参数为UIVariable
    static Dictionary<string, Action<UIVariable>> eventAction;

    static TableToValue()
    {
        eventAction = new Dictionary<string, Action<UIVariable>>();
    }

    public static void FireEvent(string key,UIVariable variable)
    {
        if (eventAction.ContainsKey(key))
        {
            eventAction[key](variable);
        }
    }

    public static void RegistEvent(Action<UIVariable> action,string key)
    {
        if (eventAction.ContainsKey(key))
        {
            eventAction[key] += action;
            return;
        }
        eventAction.Add(key, action);
    }

    public static void UnRegistEvent(string key)
    {
        if (eventAction.ContainsKey(key))
        {
            eventAction.Remove(key);
        }
    }
}
