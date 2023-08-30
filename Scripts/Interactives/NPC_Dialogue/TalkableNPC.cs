
using UnityEngine;

/// <summary>
/// Represents an NPC that is able to engage in dialogue with the player.
/// <para /> Inherits from BasicInteractive.
/// </summary>
public class TalkableNPC : BasicInteractive
{
    [Header("My Character Info")]
    public Character MyCharacter;


    /// <summary>
    /// Invokes the DialogueStartEvent and returns null.
    /// </summary>
    /// <returns>Null.</returns>
    public override ICommand QueryInteraction()
    {
        base.QueryInteraction();

        PromptConversation();

        return null;
    }

    public void PromptConversation()
    {
        DialogueSystem.PromptConversation(MyCharacter);
    }
}
