
using UnityEngine;

/// <summary>
/// An attribute to conditionally show or hide fields depending on 
/// another's value.
/// </summary>
public class ShowIfAttribute : PropertyAttribute
{
    public string ShowTestFieldName;

    /// <summary>
    /// Only shows this property if the property names
    /// <paramref name="showTestFieldName"/> is a non-default value. <para />
    /// </summary>
    /// <param name="showTestFieldName"> 
    /// The name of the field this field is dependent on. <para />
    /// It's HIGHLY recommended to use the <c>nameof</c> operator to get the name of the field.
    /// </param>
    public ShowIfAttribute(string showTestFieldName)
    {
        ShowTestFieldName = showTestFieldName;
    }
}
