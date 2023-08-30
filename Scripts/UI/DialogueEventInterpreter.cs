using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueEventInterpreter : MonoBehaviour
{
    [Header("Event")]
    public DialogueSOEvent EventToListenTo;

    [Header("Dependencies")]
    public Image FaceImage;
    public TMP_Text NameText;
    public TMP_Text DialogueText;

    void Start()
    {
        // Add a listener to the event to trigger the SetUI method
        EventToListenTo.AddListener(SetUI);
    }

    private void SetUI(Dialogue dialogue)
    {
        // Update the UI elements with the data from the dialogue
        FaceImage.sprite = dialogue.Character.Icon;
        NameText.SetText(dialogue.Character.ReferredToAs);
        DialogueText.SetText(dialogue.LineText);
    }
}
