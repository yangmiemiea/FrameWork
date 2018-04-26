using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableBind : BaseBehaviour {

    public VariableTable table;

    [HideInInspector]
    public UIVariable variable;

    [HideInInspector]
    public int index;
}
