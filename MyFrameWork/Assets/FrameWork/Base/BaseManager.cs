using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有单例的基类
/// </summary>
public class BaseManager<T> : MonoBase where T : BaseManager<T>
{
    public Action<MsgBase> handler;

    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if(instance == null)
                {
                    instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }
                instance.Init();
            }
            return instance;
        }
    }

    protected override void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// 提供给子类初始化接口
    /// </summary>
    protected virtual void Init()
    {
        
    }

    public Dictionary<ushort, Action<MsgBase>> eventTree = new Dictionary<ushort, Action<MsgBase>>();

    /// <summary>
    /// 处理事件 上层MsgCenter收到消息通过该接口转发到各个Manager
    /// </summary>
    /// <param name="tmpMsg"></param>
    public override void ProcessEvent(MsgBase tmpMsg)
    {
        if (!eventTree.ContainsKey(tmpMsg.msgId))
        {
            Debug.LogError("msg not be registed == " + tmpMsg.msgId);
            Debug.LogError("Msg Manager == " + tmpMsg.GetManager());
        }
        else
        {
            eventTree[tmpMsg.msgId](tmpMsg);
        }
    }

    /// <summary>
    /// 注册协议
    /// </summary>
    /// <param name="protocol"></param>
    /// <param name="receiver"></param>
    public void Register(MsgBase tmpMsg, Action<MsgBase> handler)
    {
        if (eventTree.ContainsKey(tmpMsg.msgId))
        {
            if (eventTree[tmpMsg.msgId] == null)
                eventTree[tmpMsg.msgId] = handler;
            else
                eventTree[tmpMsg.msgId] += handler;
        }
        else
        {
            eventTree.Add(tmpMsg.msgId, handler);
        }
    }

    public void UnRegister(MsgBase tmpMsg, Action<MsgBase> handler)
    {
        if (eventTree.ContainsKey(tmpMsg.msgId))
        {
            if (eventTree[tmpMsg.msgId] != null)
                eventTree[tmpMsg.msgId] -= handler;
            else
                eventTree.Remove(tmpMsg.msgId);
        }
    }

    public void UnRegister(MsgBase tmpMsg)
    {
        if (eventTree.ContainsKey(tmpMsg.msgId))
        {
            eventTree[tmpMsg.msgId] = null;
            eventTree.Remove(tmpMsg.msgId);
        }
    }

}
