using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariableBindText : VariableBind {

    public string text;
    private Text UIText;

    protected override void Awake()
    {
        TableToValue.RegistEvent((UIVariable variable) =>
        {
            this.UIText = GetComponent<Text>();

            this.variable = variable;

            if (this.text == "")
            {
                UIText.text = (string)this.variable.value;
            }
            else
            {
                UIText.text = string.Format(this.text, this.variable.value);
            }
        }, this.variable.name);
    }
}
