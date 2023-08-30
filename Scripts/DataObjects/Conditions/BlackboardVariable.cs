using UnityEngine;
using UnityEngine.Events;
using System;

[CreateAssetMenu(fileName = "New Blackboard Variable", menuName = "Minotaur/New Blackboard Variable", order = -1)]
public class BlackboardVariable : ScriptableObject
{
    // Event triggered when the value of the variable changes
    [HideInInspector]
    public UnityEvent<int> OnValueChanged = new UnityEvent<int>();

    // The value of the variable
    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (_value == value)
                return;

            _value = value;
            OnValueChanged.Invoke(value);
        }
    }

    [NonSerialized] private int _value;
}
