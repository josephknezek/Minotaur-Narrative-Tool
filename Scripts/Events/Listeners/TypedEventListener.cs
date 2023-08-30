using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TypedEventListener<TListener> : MonoBehaviour
{
    [System.Serializable]
    private class TypedEventPair<T>
    {
        [Tooltip("Purely for organizational purposes")]
        public string Note = "";

        [Header("Event Information")]
        public TypedSOEvent<T> EventToListenTo;
        public UnityEvent<T> OnEventTriggered = new UnityEvent<T>();

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
    private List<TypedEventPair<TListener>> TypedEvents;


    void OnEnable()
    {
        foreach (var pair in TypedEvents)
        {
            pair.Subscribe();
        }
    }

    // Unsubscribes from each basic event pair when disabled.
    private void OnDisable()
    {
        foreach (var pair in TypedEvents)
        {
            pair.Unsubscribe();
        }
    }
}