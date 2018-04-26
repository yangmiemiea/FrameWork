using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseCtrl {

    private BaseCtrl instance;

    public BaseCtrl Instance
    {
        get
        {
            if (instance == null)
            {              
                instance = new BaseCtrl();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public BaseCtrl()
    {
        this.__init();
    }

    public virtual void __init() { }

    public virtual void __delete() { }

    public void DeleteMe()
    {
        __delete();
    }

   

    /// <summary>
    /// 绑定全局事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="evetn"></param>
    public void BindGlobalEvent(GlobalEventType evetnType,UnityAction action)
    {
        GlobalEventSystem.Bind(evetnType, action);
    }

    public void UnBind(GlobalEventType evetnType)
    {
        GlobalEventSystem.UnBind(evetnType);
    }

    public void UnBind(GlobalEventType evetnType,UnityAction action)
    {
        GlobalEventSystem.UnBind(evetnType, action);
    }

    public void Fire(GlobalEventType evetnType)
    {
        GlobalEventSystem.Fire(evetnType);
    }

    /// <summary>
    /// 注册协议
    /// </summary>
    /// <param name="protocol">协议</param>
    /// <param name="receiver">协议返回时的回调</param>
    public void Register(MsgBase tmpMsg, Action<MsgBase> handler)
    {
        ViewManager.Instance.Register(tmpMsg, handler);
    }

    public void UnRegister(MsgBase tmpMsg, Action<MsgBase> handler)
    {
        ViewManager.Instance.UnRegister(tmpMsg, handler);
    }

    public void UnRegister(MsgBase tmpMsg)
    {
        ViewManager.Instance.UnRegister(tmpMsg);
    }
}

