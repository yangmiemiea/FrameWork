using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class MonoBase : BaseBehaviour
{
    /// <summary>
    /// 消息处理
    /// </summary>
    /// <param name="tmpMsg"></param>
    public abstract void ProcessEvent(MsgBase tmpMsg);
}
