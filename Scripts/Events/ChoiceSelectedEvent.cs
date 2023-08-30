using Unity.VisualScripting;

// Custom event unit class for handling choice selected events in Unity Visual Scripting.
[UnitTitle("On Choice Selected")]
[UnitCategory("Dialogue System")]
public class ChoiceSelectedEvent : EventUnit<int>
{
    [DoNotSerialize] // No need to serialize ports.
    public ValueOutput Selection { get; private set; } // The Event output data to return when the Event is triggered.
    protected override bool register => true;

    // Get the event hook for the choice selection event.
    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook(DialogueSystem.ChoiceSelectionEventString);
    }

    protected override void Definition()
    {
        base.Definition();
        Selection = ValueOutput<int>(nameof(Selection)); // Define the output port for the selected choice.
    }

    // Assign the selected choice to the output port.
    protected override void AssignArguments(Flow flow, int data)
    {
        flow.SetValue(Selection, data);
    }
}
