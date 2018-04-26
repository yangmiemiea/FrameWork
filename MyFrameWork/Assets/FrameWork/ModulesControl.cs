using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModulesControl {

    public static void __init(bool isQuickLogin = false)
    {
        CreateCoreModule();
        if (!isQuickLogin)
        {
            CreateLoginModule();
        }
        CreateGameModule();
    }

    /// <summary>
    /// 初始化各Manager
    /// </summary>
    public static void CreateCoreModule()
    {
        //BaseManager已做处理
    }

    public static void CreateLoginModule()
    {

    }

    static List<BaseCtrl> gameModule = new List<BaseCtrl>();
    /// <summary>
    /// 初始化Ctrl
    /// </summary>
    public static void CreateGameModule()
    {
        gameModule.Add(new MainCtrl());
    }

    public static void __delete()
    {
        for (int i = 0; i < gameModule.Count; i++)
        {
            gameModule[i].DeleteMe();
        }
    }
}
