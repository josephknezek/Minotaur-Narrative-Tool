using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Minotaur/Quests/New Quest", fileName = "New Quest")]
[Inspectable]
public class Quest : ScriptableObject
{   
    public Character AssignedBy;

    [TextBox]
    public string QuestDescription = "UNDEFINED";

    [Space, Tooltip("A variable to track if this quest has been completed or not.")]
    public BlackboardVariable QuestCompleteVariable;

    [Tooltip("Will automatically be retrieved if quest progression events are " +
        "derived from this asset's base path.\n\n" +
        "For example, a quest in Assets/Quests/ThisQuest will retrieve events " +
        "from Assets/Quests/ThisQuest/Events")]
    public List<BasicSOEvent> QuestProgressionEvents;

    [HideInInspector]
    public QuestStep ActiveStep;

    // A list of all the quests that have been activated for this user.
    public static List<Quest> BackloggedQuests = new List<Quest>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        GetProgressionEvents();
    }

    public void GetProgressionEvents()
    {
        string path = AssetDatabase.GetAssetPath(this);
        path = path.Replace(Path.GetFileName(path), "");

        QuestProgressionEvents = AssetDatabaseExtensions.GetAssetsOfTypeRecursivelyFromPath<BasicSOEvent>(path);
    }
#endif

    public void SetQuestActive()
    {
        if (!ActiveStep.Active)
            ActiveStep.Active = true;
    }

    public void CompleteQuest()
    {
        ActiveStep.Active = false;

        // Complete the quest!
        if (QuestCompleteVariable != null)
            QuestCompleteVariable.Value = 1;

        // Remove it from the backlog
        if (BackloggedQuests.Contains(this))
            BackloggedQuests.Remove(this);

        // No more quests
        if (BackloggedQuests.Count == 0)
        {
            QuestStep.ActiveQuestStep = null;
            StaticQuestEvents.InvokeNoQuest();
        }
        else // Choose next backlogged
        {
            BackloggedQuests.First().ActiveStep.Active = true;
        }
    }
}
