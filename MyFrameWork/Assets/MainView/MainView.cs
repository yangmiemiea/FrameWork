using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainView : BaseView {

	public MainView(string viewName) :base(viewName)
    {
        Debug.Log("MainView");
    }

    public override void __init()
    {
        Debug.Log("__init");
    }

    public override void __delete()
    {
        Debug.Log("__delete");
    }

    UIVariable text;
    public override void LoadCallBack()
    {
        Debug.Log("LoadCallBack");
        ListenEvent("Click", Click);
        ListenEvent("ClickClose", ClickClose);

        text = FindVariable("text");
        text.SetVlaue("这是主界面");
    }

    public override void ReleaseCallBack()
    {
        Debug.Log("ReleaseCallBack");
        text = null;
    }

    public override void OpenCallBack()
    {
        Debug.Log("OpenCallBack");
    }

    public override void CloseCallBack()
    {
        Debug.Log("CloseCallBack");
    }

    public void Click()
    {
        Debug.Log("点击按钮");
    }

    public void ClickClose()
    {
        Close();
    }
}
