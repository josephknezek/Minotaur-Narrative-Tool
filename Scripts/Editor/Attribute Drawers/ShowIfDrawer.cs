using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    private int _heightMod = 0;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * _heightMod;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        _heightMod = 0;
        ShowIfAttribute showif = attribute as ShowIfAttribute;
        Type type = fieldInfo.DeclaringType;
        SerializedProperty prop = property.serializedObject.FindProperty(showif.ShowTestFieldName);
        FieldInfo info = type.GetField(showif.ShowTestFieldName);

        if (prop == null)
        {
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Italic;
            EditorGUI.LabelField(position, $"No value {showif.ShowTestFieldName} found.", style);
            return;
        }

        object val = info.GetValue(property.serializedObject.targetObject);
        
        bool hasNonDefaultValue = false;
        if (val != null)
        {
            Type showFieldType = val.GetType();
            
            object defaultValue = Activator.CreateInstance(showFieldType);

            hasNonDefaultValue = !val.Equals(defaultValue);
        }

        if (!hasNonDefaultValue)
            return;

        _heightMod = 1;

        EditorGUI.PropertyField(position, property, label);
    }
}
