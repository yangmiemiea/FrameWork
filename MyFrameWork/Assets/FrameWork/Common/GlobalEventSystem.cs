using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 全局事件
/// </summary>
public static class GlobalEventSystem {

    private static Dictionary<string, UnityAction> actionTree = new Dictionary<string, UnityAction>();

    /// <summary>
    /// 注册全局事件
    /// </summary>
    /// <param name="type"></param>
    /// <param name="call"></param>
    public static void Bind(GlobalEventType type, UnityAction call)
    {
        if (actionTree.ContainsKey(type.ToString()))
        {
            if (actionTree[type.ToString()] == null)
            {
                actionTree[type.ToString()] = call;
            }
            else
            {
                actionTree[type.ToString()] += call;
            }         
        }
        else
        {
            actionTree.Add(type.ToString(), call);
        }
    }

    /// <summary>
    /// 注销某类全局事件
    /// </summary>
    /// <param name="type"></param>
    public static void UnBind(GlobalEventType type)
    {
        if (actionTree.ContainsKey(type.ToString()))
        {
            UnityAction tmpCall = actionTree[type.ToString()];
            tmpCall = null;
            actionTree.Remove(type.ToString());
        }
        else
        {
            Debug.LogError("UnBind Error, no such type == " + type.ToString());
        }
    }

    /// <summary>
    /// 注销单个全局事件
    /// </summary>
    /// <param name="type"></param>
    /// <param name="call"></param>
    public static void UnBind(GlobalEventType type, UnityAction call)
    {
        if (actionTree.ContainsKey(type.ToString()))
        {
            UnityAction tmpCall = actionTree[type.ToString()];
            if (tmpCall != null)
            {
                try
                {
                    tmpCall -= call;
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                if (tmpCall == null)
                {
                    actionTree.Remove(type.ToString());
                }
            }
        }
        else
        {
            Debug.LogError("UnBind Error, no such call be bind == " + call.Method.Name);
        }
    }

    /// <summary>
    /// 响应全局事件
    /// </summary>
    /// <param name="type">全局事件类型</param>
    public static void Fire(GlobalEventType type)
    {
        if (actionTree.ContainsKey(type.ToString()))
        {
            if (actionTree[type.ToString()] != null)
            {
                actionTree[type.ToString()]();
            }
        }
        else
        {
            Debug.LogError("No Such Event == " + type.ToString());
        }
    }

    /// <summary>
    /// 注销所有全局事件
    /// </summary>
    public static void UnBindAll()
    {
        actionTree.Clear();
    }
}
