using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;

[Serializable, Inspectable]
public class Conversation
{
    public int ConversationID;               // The ID of the conversation
    public Character TriggerCharacter;       // The character triggering the conversation
    public int Priority;                     // The priority of the conversation
    public bool Repeatable;                  // Whether the conversation is repeatable
    public List<Conditional> Conditions;     // List of conditions for the conversation to be viable

    public bool Viable
    {
        get => _viable;
        private set
        {
            if (_viable == value)
                return;

            _viable = value;

            if (_viable)
                DialogueSystem.AddViableConversation(this);
            else
                DialogueSystem.RemoveViableConversation(this);
        }
    }

    private bool _viable = false;

    public Conversation(int id, Character triggerChar, int priority, bool repeatable, List<Conditional> conditions)
    {
        ConversationID = id;
        TriggerCharacter = triggerChar;
        Priority = priority;
        Repeatable = repeatable;
        Conditions = conditions;

        // If any condition changes, see if this conversation is viable to be chosen.
        foreach (var condition in Conditions)
        {
            condition.Initialize();
            condition.OnConditionChanged.AddListener(EvaluateViability);
        }

        EvaluateViability();
    }

    private void EvaluateViability()
    {
        // Check every condition
        foreach (var condition in Conditions)
        {
            // If it's true, do nothing
            if (condition.CurrentCondition)
                continue;

            // Otherwise, this conversation is not viable
            Viable = false;
            return;
        }

        // If every condition is true, this conversation is viable
        Viable = true;
    }
}
