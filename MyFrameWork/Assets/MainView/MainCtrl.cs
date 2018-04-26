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
}
