using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UIComponent))]
public class UIComponentDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (new EditorGUI.PropertyScope(position, label, property))
        {
            //反射拿到name成员
            var nameProperty = property.FindPropertyRelative("name");
            //拿到序列化的属性
            SerializedProperty gameProperty = property.FindPropertyRelative("gameObject");
            var nameRect = new Rect(position)
            {
                width = position.width * 0.4f,
            };
            var gameRect = new Rect(position)
            {
                width = position.width * 0.6f - 5,
                x = position.width * 0.4f + 38,
            };
            nameProperty.stringValue = EditorGUI.TextField(nameRect, nameProperty.stringValue);
            gameProperty.objectReferenceValue = EditorGUI.ObjectField(gameRect, gameProperty.objectReferenceValue, typeof(GameObject), true);
        }
    }
}
