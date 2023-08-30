using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a script that listens for basic events and invokes their associated listeners when enabled.
/// <para /> Inherits from MonoBehaviour.
/// </summary>
public class BasicEventListener : MonoBehaviour
{
    [System.Serializable]
    private class BasicEventPair
    {
        [Tooltip("Purely for organizational purposes")]
        public string Note = "";

        [Header("Event Information")]
        public BasicSOEvent EventToListenTo;

        [SerializeField]
        public UnityEvent OnEventTriggered = new UnityEvent();

        /// <summary>
        /// Subscribes to the BasicSOEvent.
        /// </summary>
        public void Subscribe()
        {
            EventToListenTo.AddListener(OnEventTriggered.Invoke);
        }

        /// <summary>
        /// Unsubscribes from the BasicSOEvent.
        /// </summary>
        public void Unsubscribe()
        {
            EventToListenTo.RemoveListener(OnEventTriggered.Invoke);
        }
    }

    [SerializeField]
    private List<BasicEventPair> BasicEvents;

    // Subscribes to each basic event pair when enabled.
    void OnEnable()
    {
        foreach (var pair in BasicEvents)
        {
            pair.Subscribe();
        }
    }

    // Unsubscribes from each basic event pair when disabled.
    private void OnDisable()
    {
        foreach (var pair in BasicEvents)
        {
            pair.Unsubscribe();
        }
    }
}

