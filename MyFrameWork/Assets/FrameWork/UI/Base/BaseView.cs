using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 基础view类 所有View都继承自baseview
/// </summary>
public class BaseView {

    private bool is_open;

    private TimeEvent deleteTimer;

    string viewName;

    EventTable eventTable;

    VariableTable variableTable;

    NameTable nameTable;

    public string bundleName;

    public GameObject viewGameobject;

    /// <summary>
    /// 初始化界面
    /// </summary>
    /// <param name="vieName"></param>
    public BaseView(string viewName)
    {
        this.viewName = viewName;
        ViewManager.Instance.RegisterView(viewName, this);
    }

    public virtual void PrefabLoadCallBack(GameObject go)
    {
        viewGameobject = go;
        eventTable = viewGameobject.GetComponent<EventTable>();
        variableTable = viewGameobject.GetComponent<VariableTable>();
        nameTable = viewGameobject.GetComponent<NameTable>(); ;

        __init();
        LoadCallBack();
        OpenCallBack();
    }

    public void Open()
    {
        if (deleteTimer != null)
        {
            GlobalTimeRequest.CancleTime(deleteTimer);
            deleteTimer = null;
        }
        if (viewGameobject != null)
        {
            viewGameobject.SetActive(true);
            viewGameobject.transform.localScale = new Vector3(1, 1, 1);
            viewGameobject.transform.position = Game.Instance.UILayer.position;
            OpenCallBack();
        }
    }

    public virtual void Flush(params object[] paramsList)
    {
        OnFlush(paramsList);
    }

    /// <summary>
    /// 销毁自身
    /// </summary>
    private void DestroySelf()
    {
        GameObject.Destroy(viewGameobject);
        this.viewGameobject = null;
        ViewManager.Instance.RemoveOpen(this);
        this.ReleaseCallBack();
        this.__delete();
    }

    public bool IsOpen()
    {
        return is_open;
    }

    #region 子类调用

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

    public void ListenEvent(string eventName, UnityAction listener)
    {
        try
        {
            eventTable.ListenEvent(eventName, listener);
        }
        catch (Exception e)
        {

            Debug.LogError(e.ToString());
        }      
    }

    public void ClearEvent(string eventName)
    {
        if (eventTable != null)
            eventTable.ClearEvent(eventName);
    }

    public void ClearAllEvent()
    {
        if (eventTable != null)
            eventTable.ClearAllEvent();
    }

    public void Close()
    {
        viewGameobject.SetActive(false);
        CloseCallBack();
        deleteTimer = GlobalTimeRequest.AddDelayTime(5, DestroySelf);
    }

    public virtual void OnFlush(params object[] paramsList)
    {

    }

    #endregion

    #region 子类继承

    /// <summary>
    /// 预制件加载完成时调用
    /// </summary>
    public virtual void __init()
    {

    }

    public virtual void __delete()
    {

    }

    public virtual void LoadCallBack()
    {

    }

    public virtual void ReleaseCallBack()
    {

    }

    public virtual void OpenCallBack()
    {

    }

    public virtual void CloseCallBack()
    {

    }
    #endregion
}
