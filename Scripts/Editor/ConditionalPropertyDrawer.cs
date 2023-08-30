
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Conditional))]
public class ConditionalPropertyDrawer : PropertyDrawer
{
    // Override the OnGUI method to customize the GUI rendering of the Conditional property
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Calculate the positions for the fields within the property drawer
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var bbRect = new Rect(position.x, position.y, position.width * 0.5f, position.height);
        var evalRect = new Rect(position.x + position.width * 0.51f, position.y, position.width * 0.28f, position.height);
        var compRect = new Rect(position.x + position.width * 0.80f, position.y, position.width * 0.2f, position.height);

        // Render the property fields using EditorGUI.PropertyField
        EditorGUI.PropertyField(bbRect, property.FindPropertyRelative("BlackboardReference"), GUIContent.none);
        EditorGUI.PropertyField(evalRect, property.FindPropertyRelative("MyEvaluator"), GUIContent.none);
        EditorGUI.PropertyField(compRect, property.FindPropertyRelative("ComparisonValue"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}