using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableBindActive : VariableBind {
    public bool IsActive;

    public bool IsReverse;

    protected override void Awake()
    {
        TableToValue.RegistEvent((UIVariable variable) =>
        {
            this.variable = variable;
            IsActive = (Boolean)variable.value;
            gameObject.SetActive(IsReverse ? !IsReverse : IsActive);
        }, this.variable.name);
    }

    protected override void OnDestroy()
    {
        TableToValue.UnRegistEvent(this.variable.name);
    }
}
