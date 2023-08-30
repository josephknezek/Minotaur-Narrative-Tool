using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public abstract class TypedSOEvent<T> : ScriptableObject
{
    private event Action<T> _onTrigger;

    public void Invoke(T input)
    {
        _onTrigger?.Invoke(input);
    }

    public void AddListener(Action<T> callback)
    {
        _onTrigger += callback;
    }

    public void RemoveListener(Action<T> callback)
    {
        _onTrigger -= callback;
    }
}