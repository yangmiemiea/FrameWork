using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(NameTable))]

public class NameTableEditor : Editor {

    ReorderableList list;

    private void OnEnable()
    {
        //通过反射找到NameTable 中 component成员
        var prop = serializedObject.FindProperty("component");
        list = new ReorderableList(serializedObject, prop);
        list.drawElementCallback = 
            (rect, index, isActive, isFocused) =>
        {
            var element = prop.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element);
        };
        list.drawHeaderCallback = (rect) =>
        {
            EditorGUI.LabelField(rect, "Name", "Widget");
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

}
