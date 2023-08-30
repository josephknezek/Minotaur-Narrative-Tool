
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LabelAsAttribute))]
public class LabelAsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        LabelAsAttribute labelAs = attribute as LabelAsAttribute;

        EditorGUI.PropertyField(position, property, new GUIContent(labelAs.LabelOverride));
    }
}
