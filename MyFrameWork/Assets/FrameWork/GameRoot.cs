using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : BaseManager<GameRoot>
{

    protected override void Awake()
    {
        //初始化ctrl
        ModulesControl.__init();
        gameObject.AddComponent<GlobalTimeRequest>();
    }

    protected override void OnEnable()
    {
        //初始化预制件
        ViewManager.Instance.InitViewObject();
    }

    protected override void Start()
    {       
        ViewManager.Instance.Open(ViewName.MainView);
    }

    protected override void OnApplicationQuit()
    {

    }

    protected override void OnDestroy()
    {

    }
}
