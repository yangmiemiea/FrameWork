using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameTool {
    public const int MsgSpan = 3000;
}

public enum ManagerID
{
    GameManager = 0,
    ViewManager = FrameTool.MsgSpan,
    AudioManager = FrameTool.MsgSpan * 2,
    NPCManager = FrameTool.MsgSpan * 3,
    CharacterManager = FrameTool.MsgSpan * 4,
    AssetManager = FrameTool.MsgSpan * 5,
    NetManager = FrameTool.MsgSpan * 6,
}