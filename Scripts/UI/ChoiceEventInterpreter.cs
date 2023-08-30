using UnityEngine;
using TMPro;

public class ChoiceEventInterpreter : MonoBehaviour
{
    [Header("Event")]
    public ChoiceSOEvent ChoiceEvent;
    public DialogueSOEvent DialogueEvent;

    [Header("Canvas")]
    public GameObject DialoguePanel;
    public GameObject ChoicePanel;
    public TMP_Text PreviousNameText;
    public TMP_Text PreviousLineText;

    [Header("Choice 1 UI")]
    public GameObject Choice1;
    public TMP_Text Choice1Text;

    [Header("Choice 2 UI")]
    public GameObject Choice2;
    public TMP_Text Choice2Text;

    [Header("Choice 3 UI")]
    public GameObject Choice3;
    public TMP_Text Choice3Text;

    [Header("Choice 4 UI")]
    public GameObject Choice4;
    public TMP_Text Choice4Text;

    void Start()
    {
        // Add listeners to the ChoiceEvent to handle UI updates and show choices
        ChoiceEvent.AddListener((x) => DialoguePanel.SetActive(false));
        ChoiceEvent.AddListener((x) => ChoicePanel.SetActive(true));
        ChoiceEvent.AddListener(ShowChoices);

        // Add a listener to the DialogueEvent to update the previous text
        DialogueEvent.AddListener(SetLastText);
    }

    private void ShowChoices(Choice choice)
    {
        // Hide all choice objects initially
        Choice1.SetActive(false);
        Choice2.SetActive(false);
        Choice3.SetActive(false);
        Choice4.SetActive(false);

        // Loop through the choice texts and show the corresponding choice objects
        for (int i = 0; i < choice.ChoiceTexts.Count; i++)
        {
            switch (i)
            {
                case 0:
                    Choice1.SetActive(true);
                    Choice1Text.SetText(choice.ChoiceTexts[i]);
                    break;
                case 1:
                    Choice2.SetActive(true);
                    Choice2Text.SetText(choice.ChoiceTexts[i]);
                    break;
                case 2:
                    Choice3.SetActive(true);
                    Choice3Text.SetText(choice.ChoiceTexts[i]);
                    break;
                case 3:
                    Choice4.SetActive(true);
                    Choice4Text.SetText(choice.ChoiceTexts[i]);
                    break;
                default:
                    break;
            }
        }
    }

    private void SetLastText(Dialogue dialogue)
    {
        // Update the previous text with the provided dialogue information
        PreviousLineText.SetText(dialogue.LineText);
        PreviousNameText.SetText(dialogue.Character.ReferredToAs);
    }
}
