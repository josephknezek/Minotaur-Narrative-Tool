
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;
using System;

public class ScriptingEventListener : MonoBehaviour
{
    public List<ScriptingEventPair> ScriptingEvents;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize each ScriptingEventPair
        foreach (var pair in ScriptingEvents)
        {
            pair.Initialize();
        }
    }
}

/// <summary>
/// A pair of a BasicSOEvent and a UnityEvent to invoke when the BasicSOEvent is triggered.
/// <para /> Inherits from System.Serializable.
/// </summary>
[System.Serializable]
public class ScriptingEventPair
{
    [Tooltip("Purely for organizational purposes")]
    public string Note = "";

    [Header("Event Information")]
    public string EventToListenFor;
    public UnityEvent OnEventTriggered = new UnityEvent();

    private Action<string> Check;

    public void Initialize()
    {
        // Create the CheckAction and subscribe to the BasicSOEvent
        Check = new Action<string>(CheckEvent);
        Subscribe();
    }

    /// <summary>
    /// Subscribes to the BasicSOEvent.
    /// </summary>
    private void Subscribe()
    {
        EventBus.Register(EventToListenFor, Check);
    }

    /// <summary>
    /// Unsubscribes from the BasicSOEvent.
    /// </summary>
    public void Unsubscribe()
    {
        EventBus.Unregister(EventToListenFor, Check);
    }

    private void CheckEvent(string ev)
    {
        // Check if the received event matches the event to listen for, and invoke the UnityEvent if true
        if (ev == EventToListenFor)
            OnEventTriggered.Invoke();
    }
}
