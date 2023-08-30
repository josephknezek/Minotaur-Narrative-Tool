using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEventRouter : SOEventRouter
{

    [SerializeField]
    private Quest MyQuest;

    protected override void OnValidate()
    {
        base.OnValidate();

        if (MyQuest == null)
            return;

        basicRoutedEvents.Clear();

#if UNITY_EDITOR
        MyQuest.GetProgressionEvents();
#endif

        foreach (var item in MyQuest.QuestProgressionEvents)
        {
            basicRoutedEvents.Add(new BasicRoutedEventPair(item.name, item));
        }
    }

}
