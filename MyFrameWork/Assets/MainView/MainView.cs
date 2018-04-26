using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    GameObject tab1;
    GameObject tab2;
    GameObject tab3;
    Tab1View tab1_view;
    Tab2View tab2_view;
    Tab3View tab3_view;
    public override void LoadCallBack()
    {
        Debug.Log("LoadCallBack");
        ListenEvent("Click", Click);
        ListenEvent("ClickClose", ClickClose);

        text = FindVariable("text");

        tab1 = FindObj("Tab1");
        tab2 = FindObj("Tab2");
        tab3 = FindObj("Tab3");

        AddToggleValueChangedListener(tab1, OnToggleChange, (int)TabIndex.Test1View);
        AddToggleValueChangedListener(tab2, OnToggleChange, (int)TabIndex.Test2View);
        AddToggleValueChangedListener(tab3, OnToggleChange, (int)TabIndex.Test3View);

        GameObject tab1_obj = FindObj("Tab1Content");
        GameObject tab2_obj = FindObj("Tab2Content");
        GameObject tab3_obj = FindObj("Tab3Content");
        tab1_obj.GetComponent<UIPrefabLoader>().Wait(
            (obj) => {
                tab1_view = new Tab1View(obj);
        });
        tab2_obj.GetComponent<UIPrefabLoader>().Wait(
           (obj) => {
               tab2_view = new Tab2View(obj);
           });
        tab3_obj.GetComponent<UIPrefabLoader>().Wait(
           (obj) => {
               tab3_view = new Tab3View(obj);
           });

        //    local inlay_content = self:FindObj("Tab1")

        //inlay_content.uiprefab_loader:Wait(function(obj)

        //    obj = U3DObject(obj)

        //    self.inlay_view = RuneInlayView.New(obj)

        //    self.inlay_view:InitView()

        //end)
    }

    public override void ShowIndexCallBack(int? index)
    {
        if (index == (int)TabIndex.Test1View) tab1_view.InitView();

        if (index == (int)TabIndex.Test2View) tab2_view.InitView();

        if (index == (int)TabIndex.Test3View) tab3_view.InitView();

    }

    public override void ReleaseCallBack()
    {
        Debug.Log("ReleaseCallBack");
        text = null;
    }

    public override void OpenCallBack()
    {
        if (tab1.GetComponent<Toggle>().isOn) OnToggleChange((int)TabIndex.Test1View);
        if (tab2.GetComponent<Toggle>().isOn) OnToggleChange((int)TabIndex.Test2View);
        if (tab3.GetComponent<Toggle>().isOn) OnToggleChange((int)TabIndex.Test3View);


        text.SetVlaue("这是主界面");
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
