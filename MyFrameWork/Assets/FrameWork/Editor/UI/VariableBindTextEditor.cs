using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

[CustomEditor(typeof(VariableBindText))]
class VariableBindTextEditor : Editor
{
    VariableBindText bind;
    public void OnEnable()
    {
        bind = (VariableBindText)target;
        bind.table = Tool.FindFather(bind.transform, a =>
        {
            return a.GetComponent<VariableTable>() != null;
        }).GetComponent<VariableTable>();
    }

    public override void OnInspectorGUI()
    {
        //赋值
        if (bind.table != null)
        {
            List<String> nameByType = new List<string>();
            for (int i = 0; i < bind.table.component.Length; i++)
            {
                if(bind.table.component[i].type == VariableTYPE.String)
                {
                    nameByType.Add(bind.table.component[i].name);
                }
            }

            string[] names = new string[nameByType.Count];
            names = nameByType.ToArray();
            bind.index = EditorGUILayout.Popup("String logic:", bind.index, names);
            if (names.Length > 0)
            {
                foreach (var item in bind.table.component)
                {
                    if (item.name == names[bind.index])
                    {
                        //进行绑定
                        bind.variable = item;
                    }
                }
            }
        }
        //刷新
        base.OnInspectorGUI();
    }
}
