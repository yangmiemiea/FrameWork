using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab1View : BaseRender
{
    GameObject go;
    public Tab1View(GameObject instance) : base(instance)
    {
         go = instance;
    }
    
    public void InitView()
    {
        Debug.Log("Tab1View");
        Debug.Log(go);
    }
}
