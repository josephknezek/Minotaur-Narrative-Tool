using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestReadout : MonoBehaviour
{

    public TMP_Text HeaderText;
    public TMP_Text BodyText;
    public Button ActivateButton;

    private Quest _myQuest;

    public void SetQuestInfo(Quest quest)
    {
        _myQuest = quest;

        HeaderText.SetText(_myQuest.name.Trim(new char[] { ' ', '_' }));
        BodyText.SetText(_myQuest.QuestDescription);

        ActivateButton.onClick.AddListener(SetMyQuestActive);
    }

    public void SetMyQuestActive()
    {
        _myQuest.SetQuestActive();
    }

}
