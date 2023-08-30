using UnityEngine;
using Unity.VisualScripting;

[CreateAssetMenu(menuName = "Minotaur/New Character", order = -2)]
[Inspectable]
public class Character : ScriptableObject
{
    /// <summary>
    /// The icon associated with the character.
    /// </summary>
    public Sprite Icon;

    public string ReferredToAs
    {
        get => overrideName ?? name;
        set => overrideName = value;
    }

    [DoNotSerialize] 
    private string overrideName = "";
}
