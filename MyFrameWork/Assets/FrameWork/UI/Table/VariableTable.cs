using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableTable : BaseBehaviour {

    [SerializeField]
    public UIVariable[] component;

    public Dictionary<string, VariableTYPE> variableDic;

    protected override void Awake()
    {
        variableDic = new Dictionary<string, VariableTYPE>();
        for (int i = 0; i < component.Length; i++)
        {
            variableDic.Add(component[i].name, component[i].type);
        }
    }

    public UIVariable FindVariable(string key)
    {
        foreach (var item in component)
        {
            if(item.name == key)
            {
                return item;
            }
        }
        return null;
    }
}

[Serializable]
public class UIVariable
{
    public string name;

    public VariableTYPE type;

    public object value;

    [HideInInspector]
    public List<GameObject> obList;

    public void SetVlaue(object value)
    {
        this.value = value;
        //刷新绑定了当前name的
        TableToValue.FireEvent(this.name, this);
    }
}

public enum VariableTYPE
{
    String,
    Bool,
    Int,
}
