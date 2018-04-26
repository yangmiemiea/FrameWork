using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCtrl : BaseCtrl {

    public MainView selfView;

    public MainData selfData;

    public override void __init()
    {
        if (Instance == null)
            Instance = new MainCtrl();
        Instance = this;
        selfData = new MainData();
        selfView = new MainView(ViewName.MainView);
    }

    public override void __delete()
    {
        Instance = null;

        if (selfData != null)
            selfData.Release();

        if (selfView != null)
            selfView.Release();
    }
}
