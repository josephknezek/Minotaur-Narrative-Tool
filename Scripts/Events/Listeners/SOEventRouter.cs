using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;

[RequireComponent(typeof(StateMachine))]
public class SOEventRouter : MonoBehaviour
{
    [Serializable]
    protected class BasicRoutedEventPair
    {
        [Tooltip("This is purely for organizational purposes.")]
        public string Note = "";

        [Tooltip("Sends an event equal to the name of this SO.")]
        public BasicSOEvent EventToRoutDown;

        public BasicRoutedEventPair(string note, BasicSOEvent eventToRout)
        {
            Note = note;
            EventToRoutDown = eventToRout;
        }
    }

    [SerializeField]
    protected List<BasicRoutedEventPair> basicRoutedEvents = new List<BasicRoutedEventPair>();

    [SerializeField]
    private StateMachine machine;


    protected virtual void OnValidate()
    {
        if (machine == null)
            machine = GetComponent<StateMachine>();
    }

    void Start()
    {
        foreach (var item in basicRoutedEvents)
        {
            // Send an event equal to the name of the SO
            item.EventToRoutDown.AddListener(() => TriggerScriptingEvent(item.EventToRoutDown.name));
        }
    }

    void TriggerScriptingEvent(string eventName)
    {
        machine.TriggerUnityEvent(eventName);
    }
}
