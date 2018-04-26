using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTable : BaseBehaviour {

    [SerializeField]
    private UIComponent[] component;

    Dictionary<string, GameObject> obDic = new Dictionary<string, GameObject>();

    protected override void Awake()
    {
        for (int i = 0; i < component.Length; i++)
        {
            obDic.Add(component[i].name, component[i].gameObject);
        }
    }

    public GameObject FindObj(string name)
    {
        if (obDic.ContainsKey(name))
        {
            return obDic[name];
        }
        else
        {
            Debug.LogError("the name '" + name + "'is not contain in this nameTable");
            return null;
        }
    }

}

[Serializable]
public class UIComponent {
    public string name;

    public GameObject gameObject;
}
