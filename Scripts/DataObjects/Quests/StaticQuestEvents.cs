using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

[CreateAssetMenu(menuName = "Minotaur/Quests/Utility/Static Quest Event Holder", fileName = SingletonName, order = 1)]
[Inspectable]
public class StaticQuestEvents : ScriptableObject
{
    private const string SingletonName = "_StaticQuestEventHolder";


    public static StringSOEvent QuestUI_Update
    {
        get => Instance._questUI_Update;
    }

    private static StringSOEvent QuestUI_NoQuest
    {
        get => Instance._questUI_NoQuest;
    }

    private static StaticQuestEvents Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<StaticQuestEvents>(SingletonName);

            return _instance;
        }
    }

    private static StaticQuestEvents _instance;

    [SerializeField]
    private StringSOEvent _questUI_Update;
    [SerializeField]
    private StringSOEvent _questUI_NoQuest;
    [SerializeField, TextBox]
    private string NoQuestText = "<i>No active quests - try talking to people for more!</i>";

    public static void InvokeNoQuest()
    {
        QuestUI_NoQuest.Invoke(Instance.NoQuestText);
    }

}
