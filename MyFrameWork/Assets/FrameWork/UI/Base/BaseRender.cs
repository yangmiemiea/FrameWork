using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseRender {

    GameObject viewGameObject;

    NameTable nameTable;

    EventTable eventTable;

    VariableTable variableTable;

    public BaseRender(GameObject instance)
    {
        viewGameObject = instance;
        nameTable = viewGameObject.GetComponent<NameTable>();
        eventTable = viewGameObject.GetComponent<EventTable>();
        variableTable = viewGameObject.GetComponent<VariableTable>();
        __init();
    }

    /// <summary>
    /// 类似于BaseView的LoadCallBack
    /// </summary>
    public virtual void __init() { }

    public virtual void __delete() { }

    public GameObject FindObj(string objName)
    {
        try
        {
            return nameTable.FindObj(objName);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return null;
        }
    }

    public void ListenEvent(string eventName,UnityAction call)
    {
        try
        {
            eventTable.ListenEvent(eventName, call);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    public UIVariable FindVariable(string variableName)
    {
        try
        {
            return variableTable.FindVariable(variableName);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return null;
        }
    }

    public virtual void Flush(params object[] paramsList)
    {
        OnFlush(paramsList);
    }

    #region 子类继承

    public virtual void OpenCallBack() { }

    public virtual void CloseCallBack() { }

    public virtual void OnFlush(params object[] paramsList) { }

    #endregion
}
