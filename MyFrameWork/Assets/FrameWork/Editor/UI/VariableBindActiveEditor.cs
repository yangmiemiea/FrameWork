using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 绘制VariableBindActive脚本
/// </summary>
[CustomEditor(typeof(VariableBindActive))]
public class VariableBindActiveEditor : Editor {

    VariableBindActive bind;

    private void OnEnable()
    {//targer为自身引用
        bind = (VariableBindActive)target;
        //查找当前引用的VariableTable
        bind.table = Tool.FindFather(bind.transform, a =>
        {
            return a.GetComponent<VariableTable>() != null;
        }).GetComponent<VariableTable>();
    }

    public override void OnInspectorGUI()
    {
        VariableTable variableTable = bind.table;
        //更新数据
        if (variableTable != null)
        { 
            List<String> nameByType = new List<String>();
            //拿到所有bool类型
            for (int i = 0; i < variableTable.component.Length; i++)
            {
                if(variableTable.component[i].type == VariableTYPE.Bool)
                {
                    nameByType.Add(variableTable.component[i].name);
                }
            }
            string[] names = new string[nameByType.Count];
            names = nameByType.ToArray();
            //将所有bool类型变量填充到界面 index（当前选择的变量索引)
            bind.index = EditorGUILayout.Popup("Boolean logic:", bind.index, names);
            //查找绑定的变量对应在VariableTable上的值
            if (name.Length > 0)
            {
                foreach (var item in variableTable.component)
                {
                    Debug.Log(item.name);
                    Debug.Log(names[bind.index]);
                    if (item.name == names[bind.index])
                    {//实际绑定
                        bind.variable = item;
                    }
                }
            }          
        }
        //刷新
        base.OnInspectorGUI();
    }
}
