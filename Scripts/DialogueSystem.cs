using UnityEngine;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using System.Linq;

[IncludeInSettings(true)]
public static class DialogueSystem
{
    public const string ConversationSelectedEventString = "ChooseConvo";
    public const string ChoiceSelectionEventString = "MadeChoice";

    #region Conversation

    public static int C_MostRecentID { get => C_ID; private set => C_ID = value; }
    public static HashSet<int> C_AssignedIDs = new HashSet<int>();

    // Dictionary to store viable conversations for each character
    private static Dictionary<Character, List<Conversation>> viableConvos = new Dictionary<Character, List<Conversation>>();
    // Dictionary to keep track of visited conversations for each character
    private static Dictionary<Character, HashSet<int>> visitedConvos = new Dictionary<Character, HashSet<int>>();
    // Dictionary to store non-repeatable conversations for each character
    private static Dictionary<Character, HashSet<int>> nonRepeatables = new Dictionary<Character, HashSet<int>>();
    private static int C_ID = 0;

    #endregion

    #region Dialogue

    public static int D_MostRecentID { get => D_ID; private set => D_ID = value; }
    public static HashSet<int> D_AssignedIDs = new HashSet<int>();

    // Dictionary to store all dialogues with their IDs
    private static Dictionary<int, Dialogue> allDialogue = new Dictionary<int, Dialogue>();
    private static int D_ID = 0;

    #endregion

    // Register a new conversation and assign an ID to it
    public static void RegisterConversation(ref Conversation newConvo)
    {
        C_AssignedIDs.Add(++C_MostRecentID);
        newConvo.ConversationID = C_MostRecentID;
    }

    // Add a viable conversation for a character
    public static void AddViableConversation(Conversation newConvo)
    {
        if (viableConvos.ContainsKey(newConvo.TriggerCharacter))
        {
            viableConvos[newConvo.TriggerCharacter].Add(newConvo);
        }
        else
        {
            viableConvos.Add(newConvo.TriggerCharacter, new List<Conversation>() { newConvo });
        }
    }

    // Remove a conversation from the list of viable conversations for a character
    public static void RemoveViableConversation(Conversation badConvo)
    {
        if (!viableConvos.ContainsKey(badConvo.TriggerCharacter))
            return;

        viableConvos[badConvo.TriggerCharacter].Remove(badConvo);
    }

    // Register a new dialogue and assign an ID to it
    public static void RegisterDialogue(ref Dialogue newDialogue)
    {
        D_AssignedIDs.Add(++D_MostRecentID);
        allDialogue.Add(D_MostRecentID, newDialogue);
        newDialogue.LineID = D_MostRecentID;
    }

    // Prompt a conversation for a character
    public static void PromptConversation(Character character)
    {
        // Check if the character has any viable conversations registered
        if (!viableConvos.ContainsKey(character))
        {
            Debug.Log($"{character} is not registered to the conversation dictionary!");
            return;
        }

        List<Conversation> convosWithChar = viableConvos[character];

        // Check if there are any conversations registered for the character
        if (convosWithChar.Count == 0)
        {
            Debug.Log($"There are no conversations registered to {character.name}!");
            return;
        }

        List<Conversation> freshConvos;

        if (!visitedConvos.ContainsKey(character))
        {
            // If the character has no visited conversations, consider all conversations as fresh
            freshConvos = new List<Conversation>(convosWithChar);
            visitedConvos.Add(character, new HashSet<int>());
            nonRepeatables.Add(character, new HashSet<int>());
        }
        else
        {
            // Filter out conversations that have already been visited by the character
            freshConvos = convosWithChar.Where(x => !visitedConvos[character].Contains(x.ConversationID)).ToList();

            // If all conversations have been visited, consider non-repeatable conversations as fresh
            if (freshConvos.Count == 0)
            {
                visitedConvos[character] = new HashSet<int>(nonRepeatables[character]);
                freshConvos = convosWithChar.Where(x => !visitedConvos[character].Contains(x.ConversationID)).ToList();

                // If there are still no fresh conversations, log a warning
                if (freshConvos.Count == 0)
                {
                    Debug.LogWarning($"Unable to get fresh conversations for {character.name}.");
                    return;
                }
            }
        }

        // Sort the fresh conversations based on priority
        freshConvos.Sort((convo1, convo2) => convo2.Priority.CompareTo(convo1.Priority));

        // Get conversations with the highest priority
        List<Conversation> highestPriorityConvos = freshConvos.Where(x => x.Priority == freshConvos.First().Priority).ToList();

        // Get a random conversation of the highest priority
        Conversation selection = highestPriorityConvos[UnityEngine.Random.Range(0, highestPriorityConvos.Count)];

        // Mark the conversation as visited
        visitedConvos[character].Add(selection.ConversationID);

        // If the conversation is not repeatable, mark it as non-repeatable
        if (!selection.Repeatable)
            nonRepeatables[character].Add(selection.ConversationID);

        // Trigger an event with the selected conversation ID
        EventBus.Trigger(ConversationSelectedEventString, selection.ConversationID);
    }


    // Events (Used in Visual Scripting)

    // Trigger a basic event
    public static void RaiseBasicSOEvent(BasicSOEvent eventToRaise)
    {
        eventToRaise.Invoke();
    }

    // Trigger a dialogue event with a dialogue parameter
    public static void RaiseDialogueSOEvent(TypedSOEvent<Dialogue> eventToRaise, Dialogue eventParam)
    {
        eventToRaise.Invoke(eventParam);
    }

    // Trigger a string event with a string parameter
    public static void RaiseStringSOEvent(TypedSOEvent<string> eventToRaise, string eventParam)
    {
        eventToRaise.Invoke(eventParam);
    }
}
