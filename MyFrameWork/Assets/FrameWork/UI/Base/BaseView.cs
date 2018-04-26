using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 基础view类 所有View都继承自baseview
/// </summary>
public class BaseView
{

    private bool is_open;

    private bool is_real_open;

    private TimeEvent deleteTimer;

    string viewName;

    EventTable eventTable;

    VariableTable variableTable;

    NameTable nameTable;

    public string bundleName;

    public GameObject viewGameobject;

    int def_index = 0;
    int? last_index = null;
    int show_index = -1;

    /// <summary>
    /// 初始化界面
    /// </summary>
    /// <param name="vieName"></param>
    public BaseView(string viewName)
    {   
        this.viewName = viewName;
        ViewManager.Instance.RegisterView(viewName, this);
        __init();
    }

    public virtual void PrefabLoadCallBack(GameObject go)
    {
        viewGameobject = go;
        eventTable = viewGameobject.GetComponent<EventTable>();
        variableTable = viewGameobject.GetComponent<VariableTable>();
        nameTable = viewGameobject.GetComponent<NameTable>(); ;
     
        LoadCallBack();
        GlobalTimeRequest.AddDelayTime(0.02f, OpenCallBack);
    }

    /// <summary>
    /// 单纯的打开 （未销毁又打开调用）
    /// </summary>
    /// <param name="index"></param>
    public void Open(int? index = null)
    {
        //如果未传入index  加载主界面
        is_real_open = true;

        if (index == null)
        {
            index = def_index;
        }
        if (deleteTimer != null)
        {
            GlobalTimeRequest.CancleTime(deleteTimer);
            deleteTimer = null;
        }
        if (viewGameobject != null)
        {
            if (!is_open)
            {
                SetActive(true);
                viewGameobject.transform.localScale = new Vector3(1, 1, 1);
                viewGameobject.transform.position = Game.Instance.UILayer.position;
                OpenCallBack();
            }
            else
            {
                ShowIndexCallBack(index);
            }
        }
    }

    public void SetActive(bool state)
    {
        Debug.Log(state);
        is_open = state;
        viewGameobject.SetActive(state);
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
    }

    public void Release()
    {
        ViewManager.Instance.UnRegisterView(viewName);
        __delete();
    }

    public bool IsOpen()
    {
        return is_open;
    }

    public bool IsRealOpen()
    {
        return is_real_open;
    }

    public void OnToggleChange(int index)
    {
        if (show_index == index) return;
        last_index = index;
        show_index = index;
        ShowIndexCallBack(index);
    }

    public int? GetLastIndex()
    {
        return last_index;
    }

    #region 子类调用

    /// <summary>
    /// 标签页toggle
    /// </summary>
    /// <param name="toggle"></param>
    /// <param name="listener"></param>
    /// <param name="param"></param>
    public void AddToggleValueChangedListener(GameObject toggle, UnityAction<int> listener,int tabIndex)
    {
        toggle.GetComponent<Toggle>().onValueChanged.AddListener((b) => {
            if (show_index == tabIndex) return;
            if (b) listener(tabIndex);
        });
    }

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

    public virtual void OnFlush(params object[] paramsList)
    {

    }

    public virtual void ShowIndexCallBack(int? index)
    {

    }
    #endregion
}
