using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityEditor;

[CustomPropertyDrawer(typeof(TextBoxAttribute))]
public class TextBoxDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        TextBoxAttribute box = attribute as TextBoxAttribute;
        return base.GetPropertyHeight(property, label) * (box.Lines + 1);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        TextBoxAttribute box = attribute as TextBoxAttribute;
        float labelScalar = 1 / (float)(box.Lines + 1);
        float boxScalar = box.Lines / (float)(box.Lines + 1);

        GUIStyle boxStyle = new GUIStyle(GUI.skin.textArea);
        boxStyle.wordWrap = true;

        float labelHeight = position.height * labelScalar;

        Rect labelRect = new Rect
        (
            new Vector2(position.x, position.y), 
            new Vector2(position.width, labelHeight)
        );

        Rect boxRect = new Rect
        (
            new Vector2(position.x, position.y + labelHeight),
            new Vector2(position.width, position.height * boxScalar)
        );

        EditorGUI.LabelField(labelRect, property.displayName);

        if (property.propertyType == SerializedPropertyType.String)
            property.stringValue = EditorGUI.TextArea(boxRect, property.stringValue, boxStyle);
        else
            EditorGUI.LabelField(position, label.text, "Use TextBox with strings.");
    }
}