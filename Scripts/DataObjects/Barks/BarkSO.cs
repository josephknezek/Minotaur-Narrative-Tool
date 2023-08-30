using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BarkSO : ScriptableObject
{
    // The average reading speed of English readers
    private const float WordsPerSecond = 3f;

    // A base value to wait before a bark fades
    private const float BaseWaitTime = 0.75f;

    
    public bool ShowAdvancedSettings = false;

    // Bark Trigger Info
    [Tooltip("The event that can trigger this bark.")]
    public BarkSOEvent ListenEvent;

    [Tooltip("The character who says this bark.")]
    public Character Speaker;

    [Tooltip("A pad value to make a bark more or less prioritized than it may naturally be.\n" +
        "Priority = [# of conditions] + [pad]")]
    public int PriorityPad = 0;

    [Tooltip("Whether or not this bark can play when either the character or system is on cooldown.")]
    public bool OverrideCooldowns = false;

    [Tooltip("The conditions in which this bark may be triggered.\n" +
        "Note that ALL conditions must be true for a bark to be valid.")]
    public List<Conditional> Conditions = new List<Conditional>();

    [Tooltip("Events to raise when this bark starts being displayed.")]
    public List<BasicSOEvent> OnStartedEvents = new List<BasicSOEvent>();

    // Bark Content Info
    [Tooltip("The actual text to display for this bark.")]
    public string Response = "EMPTY";

    [Min(0f), Tooltip("How long the bark text will be displayed for.\n" +
        "(Suggestion based on average English reading speed.)")]
    public float TimeToDisplay = 1.5f;

    public float _suggestedTime = 1f;

    // Bark Completion Info
    [Min(-1f), Tooltip("How long before this specific bark can be played again.\n" +
        "If set to -1, it will never play again.")] 
    public float BarkCooldown = 300f;

    [Tooltip("Variable operations to perform once this bark completes before finish events trigger.")]
    public List<Remember> OnRemember = new List<Remember>();

    [Tooltip("Events to raise when this bark finishes being displayed.")]
    public List<BasicSOEvent> OnFinishedEvents = new List<BasicSOEvent>();

    [HideInInspector]
    public UnityEvent OnCooldownComplete = new UnityEvent();

    /// <summary>
    /// The priority of this bark. Higher numbers preferred. <para />
    /// Priority = [# conditions] + [PriorityPad]
    /// </summary>
    public int Priority { get; private set; }

    /// <summary>
    /// Whether or not this bark should be played given its conditional requirements.
    /// </summary>
    public bool Viable
    {
        get => _viable;
        private set
        {
            if (_viable == value)
                return;

            _viable = value;

            if (_viable)
                RuntimeBarks.Barks.AddViableBark(this);
            else
                RuntimeBarks.Barks.RemoveViableBark(this);
        }
    }


    [NonSerialized] 
    private bool _viable = false;
    
    [NonSerialized] 
    private float _cdTimer = 0f;

    private void OnEnable()
    {
        GetSuggestedTime();
    }

    private void OnValidate()
    {
        
        GetSuggestedTime();
    }

    private void GetSuggestedTime()
    {
        // # of words / average reading speed + default display time
        // (Rounded to two decimals for readability)
        _suggestedTime = (float)Math.Round(((Response.Split(' ').Length + 1) / WordsPerSecond) + BaseWaitTime, 2);
    }

    public void RegisterToBarkList(string pathOverride = "")
    {
#if UNITY_EDITOR
        // If we have a path override, find the auto bark list at that location.
        if (pathOverride != "")
        {
            AutoBarkList list = AssetDatabase.LoadMainAssetAtPath(pathOverride) as AutoBarkList;

            if (list != null)
                list.BarkRules.Add(this);

            return;
        }
#endif

        AutoBarkList.GlobalInstance.BarkRules.Add(this);
    }

    public void UnregisterFromGlobal()
    {
        if (AutoBarkList.GlobalInstance.BarkRules.Contains(this))
            AutoBarkList.GlobalInstance.BarkRules.Remove(this);
    }

    public void Initialize()
    {
        foreach (var conditional in Conditions)
        {
            conditional.Initialize();

            conditional.OnConditionChanged.AddListener(EvaluateViability);
        }

        EvaluateViability();

        Priority = Conditions.Count + PriorityPad;
    }

    public void OnBarkStarted()
    {
        foreach (var start in OnStartedEvents)
        {
            start.Invoke();
        }
    }

    public void StartCooldown()
    {
        Viable = false;

        if (BarkCooldown < 0)
            return;

        // Get our cooldown, on top of the time it's displayed and fade time
        _cdTimer = BarkCooldown + TimeToDisplay + (BarkingCharacter.FadeTime * 2);

        RuntimeBarks.Barks.AddBarkToCooldown(this);
    }


    public void OnBarkComplete()
    {
        foreach (var remember in OnRemember)
        {
            remember.PerformOperation();
        }

        foreach (var response in OnFinishedEvents)
        {
            response.Invoke();
        }

        RuntimeBarks.Barks.StartGlobalCooldown();
    }

    public void CountdownCooldown()
    {
        _cdTimer -= Time.deltaTime;

        if (_cdTimer <= 0f)
            OnCooldownComplete.Invoke();
    }


    public void EvaluateViability()
    {
        if (_cdTimer > 0f)
        {
            Viable = false;
            return;
        }

        foreach (var item in Conditions)
        {
            if (item.CurrentCondition)
                continue;

            _viable = false;
            return;
        }

        Viable = true;
    }
}
