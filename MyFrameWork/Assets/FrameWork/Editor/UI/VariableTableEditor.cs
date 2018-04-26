using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(VariableTable))]
public class VariableTableEditor : Editor
{
    ReorderableList list;

    private void OnEnable()
    {
        var prop = serializedObject.FindProperty("component");

        list = new ReorderableList(serializedObject, prop);

        list.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            var element = prop.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element);
        };

        list.drawHeaderCallback =
            (rect) =>
            {
                EditorGUI.LabelField(rect, "Name", "Type");
            };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
