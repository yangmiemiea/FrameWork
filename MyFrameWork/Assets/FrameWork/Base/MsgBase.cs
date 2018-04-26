using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 基础消息
/// </summary>
public class MsgBase
{
    //表示65535个消息 两个字节
    public ushort msgId;

    /// <summary>
    /// 获取当前消息对应的模块
    /// </summary>
    /// <returns></returns>
    public ManagerID GetManager()
    {
        //取整
        int tmpId = msgId / FrameTool.MsgSpan;
        return (ManagerID)(tmpId * FrameTool.MsgSpan);
    }

    public MsgBase(ushort tmpMsg)
    {
        msgId = tmpMsg;
    }
}

