using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIPopulator : MonoBehaviour
{
    public GameObject QuestReadoutPrefab;

    private void OnEnable()
    {
        ClearUI();
        PopulateUI();
    }

    private void ClearUI()
    {
        foreach (Transform item in GetComponentInChildren<Transform>())
        {
            if (item == transform)
                continue;

            Destroy(item.gameObject);
        }
    }

    private void PopulateUI()
    {
        foreach (var quest in Quest.BackloggedQuests)
        {
            QuestReadout readout = Instantiate(QuestReadoutPrefab, transform).GetComponent<QuestReadout>();
            readout.SetQuestInfo(quest);
        }
    }

}
