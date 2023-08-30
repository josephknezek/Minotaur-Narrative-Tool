using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Minotaur/Quests/New Quest Step", fileName = "New Quest Step")]
[Inspectable]
public class QuestStep : ScriptableObject
{
    [Serializable]
    private class ConditionalWithDisplayText
    {
        public string Note = "";
        public Conditional MyCondition;
        public string ConditionPrefix;
        public string ConditionSuffix;

        /// <summary>
        /// Format: [Prefix] [value] / [comparison] [suffix] <para />
        /// Adds space before prefix and after suffix if each is not empty. <para \>
        /// Returns an empty string if there's no prefix or suffix.
        /// </summary>
        public string GetDisplayText()
        {
            if (ConditionPrefix != "" || ConditionSuffix != "")
                return $"{ConditionPrefix}{(ConditionPrefix == "" ? "" : " ")}" +
                    $"{MyCondition.BlackboardReference.Value} / {MyCondition.ComparisonValue}" +
                    $"{(ConditionSuffix == "" ? "" : " ")}{ConditionSuffix}";
            else
                return "";
        }
    }

    public Quest BelongsToQuest;

    [Tooltip("The text to be displayed at the head of this step's description."), TextBox]
    public string StepDisplayText;

    [SerializeField, Tooltip("A The conditions that need to be fulfilled before the quest step progresses.")]
    private List<ConditionalWithDisplayText> ProgressionConditions;
    
    [Tooltip("Whether this step should progress automatically given its conditions or " +
        "if it should only be progressed based on manual event invocations.")]
    public bool AutomaticallyProgresses = false;

    [ShowIf(nameof(AutomaticallyProgresses))]
    public BasicSOEvent StepCompleteEvent;


    public bool Active
    {
        get => _active;
        set
        {
            if (_active == value)
                return;

            _active = value;

            if (_active)
            {
                BelongsToQuest.ActiveStep = this;
                ActiveQuestStep = this;
                SubscribeConditions();
                CheckCompletion();
                TryTriggerUIUpdate();
            }
            else
            {
                UnsubConditions();
            }
        }
    }

    public static QuestStep ActiveQuestStep
    {
        get
        {
            return _activeStep;
        }
        set
        {
            if (_activeStep != null)
                _activeStep.Active = false;

            _activeStep = value;

            if (_activeStep == null)
                return;

            if (!Quest.BackloggedQuests.Contains(_activeStep.BelongsToQuest))
                Quest.BackloggedQuests.Add(_activeStep.BelongsToQuest);
        }
    }

    private static QuestStep _activeStep;

    [DoNotSerialize]
    private bool _active = false;

#if UNITY_EDITOR
    // If we don't have a quest this belongs to, try to find it.
    private void OnValidate()
    {
        if (BelongsToQuest != null)
            return;

        string path = AssetDatabase.GetAssetPath(this);
        if (string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }
        else if (System.IO.Path.GetExtension(path) != "")
        {
            path = path.Replace(System.IO.Path.GetFileName(path), "");
        }

        string searchType = $"t:{nameof(Quest)}";
        string[] guids = AssetDatabase.FindAssets(searchType, new[] { path });

        Quest main;
        if (guids.Length != 0)
        {
            main = AssetDatabase.LoadAssetAtPath<Quest>(AssetDatabase.GUIDToAssetPath(guids[0]));
            BelongsToQuest = main;
        }
    }
#endif

    public void Initialize()
    {
        Active = false;

        foreach (var item in ProgressionConditions)
        {
            item.MyCondition.Initialize();
        }
    }

    private void SubscribeConditions()
    {
        foreach (var item in ProgressionConditions)
        {
            item.MyCondition.BlackboardReference.OnValueChanged.AddListener(TryTriggerUIUpdate);
            item.MyCondition.OnConditionChanged.AddListener(CheckCompletion);
        }
    }

    private void UnsubConditions()
    {
        foreach (var item in ProgressionConditions)
        {
            item.MyCondition.BlackboardReference.OnValueChanged.RemoveListener(TryTriggerUIUpdate);
            item.MyCondition.OnConditionChanged.RemoveListener(CheckCompletion);
        }
    }


    private void TryTriggerUIUpdate(int x = 0)
    {
        if (!Active)
            return;

        string displayText = GetDisplayText();

        StaticQuestEvents.QuestUI_Update.Invoke(displayText);
    }

    public string GetDisplayText()
    {
        string displayText = $"<b>{BelongsToQuest.name.Trim(new[] { '_', ' ' })}</b>\n";
        displayText += StepDisplayText + "\n\n";

        foreach (var item in ProgressionConditions)
        {
            // Italicize if complete
            if (item.MyCondition.CurrentCondition)
                displayText += $"<i>{item.GetDisplayText()}</i>\n";
            else
                displayText += $"{item.GetDisplayText()}\n";
        }

        return displayText;
    }

    private void CheckCompletion()
    {
        foreach (var item in ProgressionConditions)
        {
            if (item.MyCondition.CurrentCondition)
                continue;

            return;
        }

        if (AutomaticallyProgresses)
            CompleteStep();
    }

    private void CompleteStep()
    {
        Active = false;

        if (StepCompleteEvent != null)
            StepCompleteEvent.Invoke();
    }

}
