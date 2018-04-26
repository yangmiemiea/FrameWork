using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 绘制单个variable对象
/// </summary>

[CustomPropertyDrawer(typeof(UIVariable))]
public class UIVariableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (new EditorGUI.PropertyScope(position, label, property))
        {
            var nameProperty = property.FindPropertyRelative("name");
            SerializedProperty gameProperty = property.FindPropertyRelative("type");
            var nameRect = new Rect(position)
            {
                width = position.width * 0.4f,
            };
            var gameRect = new Rect(position)
            {
                width = position.width * 0.6f - 5,
                x = position.width * 0.4f + 40,
            };
            //variable_name
            nameProperty.stringValue = EditorGUI.TextField(nameRect, nameProperty.stringValue);
            //type_enum
            gameProperty.intValue = (int)(VariableTYPE)EditorGUI.EnumPopup(gameRect, (VariableTYPE)gameProperty.enumValueIndex);
        }
    }
}
