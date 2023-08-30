using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

/// <summary>
/// A basic scriptable object event that can be subscribed to and triggered.
/// Inherits from ScriptableObject.
/// </summary>
[CreateAssetMenu(menuName = "Events/Basic Event", order = -1)]
[Unity.VisualScripting.Inspectable]
public class BasicSOEvent : ScriptableObject
{
    private event Action _onTrigger;

    public void Invoke()
    {
        _onTrigger?.Invoke();
    }

    public void AddListener(Action callback)
    {
        _onTrigger += callback;
    }

    public void RemoveListener(Action callback)
    {
        _onTrigger -= callback;
    }
}

