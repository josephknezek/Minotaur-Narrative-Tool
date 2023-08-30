using UnityEngine;
using Unity.VisualScripting;

public class ChoiceEventSender : MonoBehaviour
{
    // Sends the selected choice as an event using the DialogueSystem.ChoiceSelectionEventString as the event name.
    public void SendChoiceOfInt(int choice)
    {
        EventBus.Trigger(DialogueSystem.ChoiceSelectionEventString, choice);
    }
}
