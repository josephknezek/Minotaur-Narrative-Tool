
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom property drawer for the ShowOnlyAttribute.
/// </summary>
[CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
public class ShowOnlyDrawer : PropertyDrawer
{
    /// <summary>
    /// Overrides the default GUI for the property.
    /// Displays the property value in a read-only label.
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
    /// <param name="prop">SerializedProperty representing the property being drawn.</param>
    /// <param name="label">Label of the property.</param>
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        string valueString;

        switch (prop.propertyType)
        {
            case SerializedPropertyType.Boolean:
                valueString = prop.boolValue.ToString();
                break;
            case SerializedPropertyType.Integer:
                valueString = prop.intValue.ToString();
                break;
            case SerializedPropertyType.Float:
                valueString = prop.floatValue.ToString();
                break;
            case SerializedPropertyType.String:
                valueString = prop.stringValue;
                break;
            default:
                valueString = "( Not Supported )";
                break;
        }

        EditorGUI.LabelField(position, label.text, valueString);
    }
}
