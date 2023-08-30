
using Unity.VisualScripting;

// Set the title and category for the event unit to be displayed in the visual scripting editor.
[UnitTitle("On Conversation Selected")]
[UnitCategory("Dialogue System")]
public class ConversationSelectedEvent : EventUnit<int>
{
    [DoNotSerialize]
    public ValueOutput ConversationID { get; private set; }  // Output port to return the conversation ID.

    protected override bool register => true;  // Indicates that the event should be registered.

    // Retrieves the event hook for this event unit.
    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook(DialogueSystem.ConversationSelectedEventString);
    }

    // Defines the event unit's ports and properties.
    protected override void Definition()
    {
        base.Definition();
        ConversationID = ValueOutput<int>(nameof(ConversationID));  // Creates an output port named ConversationID of type int.
    }

    // Assigns the provided data to the ConversationID port using the provided Flow object.
    protected override void AssignArguments(Flow flow, int data)
    {
        flow.SetValue(ConversationID, data);
    }
}
