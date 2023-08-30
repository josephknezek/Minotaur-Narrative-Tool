using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ConditionalListener : MonoBehaviour
{
    public List<ConditionalEvent> ConditionalEvents = new List<ConditionalEvent>();

    private void Start()
    {
        foreach (var item in ConditionalEvents)
        {
            item.Initialize();
        }
    }
}

[Serializable]
public class ConditionalEvent
{
    public string Note = "";
    public List<Conditional> MyConditions;
    public bool Fulfilled
    {
        get => _fulfilled;
        set
        {
            if (_fulfilled == value)
                return;

            _fulfilled = value;

            if (_fulfilled)
                OnConditionFulfilled.Invoke();
            else
                OnConditionReverted.Invoke();
        }
    }
    public UnityEvent OnConditionFulfilled = new UnityEvent();
    public UnityEvent OnConditionReverted = new UnityEvent();

    private bool _fulfilled = false;

    public void Initialize()
    {
        foreach (var item in MyConditions)
        {
            item.Initialize();
            item.OnConditionChanged.AddListener(ConfirmFulfilled);
        }
    }

    private void ConfirmFulfilled()
    {
        foreach (var item in MyConditions)
        {
            if (!item.CurrentCondition)
            {
                Fulfilled = false;
                return;
            }
        }

        Fulfilled = true;
    }
}
