using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "New Bark List", menuName = "Minotaur/Barks/Lists/New Manual List", order = 2)]
public class ManualBarkList : ScriptableObject
{
    private class BarksByCharacter
    {
        public List<BarkSO> this[Character c]
        {
            get
            {
                if (_lines.ContainsKey(c))
                    return _lines[c];
                else
                    return null;
            }
            set
            {
                _lines[c] = value;
            }
        }

        public HashSet<Character> RegisteredCharacters = new HashSet<Character>();

        private Dictionary<Character, List<BarkSO>> _lines = 
            new Dictionary<Character, List<BarkSO>>();


        public BarksByCharacter(BarkSO bark)
        {
            AddBark(bark);
        }

        public void AddBark(BarkSO bark)
        {
            Character character = bark.Speaker;
            SetupCharacter(character);

            _lines[character].Add(bark);
        }

        public void SetupCharacter(Character character)
        {
            if (!_lines.ContainsKey(character))
            {
                _lines.Add(character, new List<BarkSO>());
                RegisteredCharacters.Add(character);
            }
        }
    }

    public bool IsOnGlobalCooldown { get => _globalCooldown > 0f; }

    [SerializeField]
    private float GlobalBarkCooldown = 15f;

    public List<BarkSO> BarkRules = new List<BarkSO>();

    [NonSerialized]
    private Dictionary<BarkSOEvent, BarksByCharacter> _viableBarks = 
        new Dictionary<BarkSOEvent, BarksByCharacter>();

    [NonSerialized]
    private List<BarkSO> _barksOnCooldown = new List<BarkSO>();

    [NonSerialized]
    private HashSet<BarkSOEvent> _uniqueEvents = new HashSet<BarkSOEvent>();

    [NonSerialized]
    private List<BarkingCharacter> _viableCharacters = new List<BarkingCharacter>();

    [NonSerialized]
    private float _globalCooldown = 0f;

    public void Initialize()
    {
        foreach (var bark in BarkRules)
        {
            bark.Initialize();
            if (!_uniqueEvents.Contains(bark.ListenEvent))
                _uniqueEvents.Add(bark.ListenEvent);
        }

        foreach (var barkEvent in _uniqueEvents)
        {
            barkEvent.AddListener(() => PromptBark(barkEvent));
        }
    }

    public void PromptBark(BarkSOEvent checkEvent)
    {
        if (!GetCanPlayBarks())
            return;

        if (!_viableBarks.ContainsKey(checkEvent))
            return;

        List<BarkSO> topBarks = new List<BarkSO>();
        HashSet<Character> availableCharacters = new HashSet<Character>();

        foreach (var person in _viableCharacters)
        {
            if (_viableBarks[checkEvent].RegisteredCharacters.Contains(person.MyCharacter))
                availableCharacters.Add(person.MyCharacter);
        }

        // Get the highest priority bark by character
        foreach (var character in availableCharacters)
        {
            if (_viableBarks[checkEvent][character].Count == 0)
                continue;

            if (IsOnGlobalCooldown)
            {
                List<BarkSO> overriddenBarks = _viableBarks[checkEvent][character].Where(x => x.OverrideCooldowns).ToList();
                if (overriddenBarks.Count == 0)
                    continue;

                overriddenBarks.Sort((x, y) => y.Priority.CompareTo(x.Priority));

                // Add all highest priority barks
                topBarks.AddRange(overriddenBarks.Where(x => x.Priority == overriddenBarks.First().Priority));
            }
            else
            {
                _viableBarks[checkEvent][character].Sort((x, y) => y.Priority.CompareTo(x.Priority));

                // Add all highest priority barks
                topBarks.AddRange(_viableBarks[checkEvent][character].Where(x => x.Priority == _viableBarks[checkEvent][character].First().Priority));
            }
        }

        if (topBarks.Count == 0)
            return;

        // Sort by priority for each bark we have
        topBarks.Sort((x, y) => y.Priority.CompareTo(x.Priority));

        List<BarkSO> bestBarks = topBarks.Where(x => x.Priority == topBarks.First().Priority).ToList();

        // Choose a random bark of the highest priorty
        BarkSO chosenBark = bestBarks[UnityEngine.Random.Range(0, bestBarks.Count)];

        // Send the bark back to the registered actor
        BarkingCharacter npc = _viableCharacters.Find(x => x.MyCharacter == chosenBark.Speaker);

        // Just in case.
        if (npc != null)
        {
            npc.SayBark(chosenBark);
            StartGlobalCooldown();
        }
    }

    public void AddBarkToCooldown(BarkSO bark)
    {
        _barksOnCooldown.Add(bark);

        bark.OnCooldownComplete.AddListener(() =>
        {
            bark.EvaluateViability();
            bark.OnCooldownComplete.RemoveAllListeners();
        });
    }

    public void CountdownCooldowns()
    {
        if (_globalCooldown > 0f)
            _globalCooldown -= Time.deltaTime;

        if (_barksOnCooldown.Count > 0)
        {
            foreach (var bark in _barksOnCooldown)
            {
                bark.CountdownCooldown();
            }
        }
    }


    public void AddViableBark(BarkSO bark)
    {
        if (_viableBarks.ContainsKey(bark.ListenEvent))
        {
            _viableBarks[bark.ListenEvent].AddBark(bark);
        }
        else
        {
            _viableBarks.Add(bark.ListenEvent, new BarksByCharacter(bark));
        }

        // Sort barks by priority
        _viableBarks[bark.ListenEvent][bark.Speaker].Sort((x, y) => y.Priority.CompareTo(x.Priority));
    }

    public void RemoveViableBark(BarkSO bark)
    {
        if (_viableBarks.ContainsKey(bark.ListenEvent))
            if (_viableBarks[bark.ListenEvent][bark.Speaker] != null)
                _viableBarks[bark.ListenEvent][bark.Speaker].Remove(bark);
    }

    public void AddViableCharacter(BarkingCharacter character)
    {
        _viableCharacters.Add(character);
    }

    public void RemoveViableCharacter(BarkingCharacter character)
    {
        if (_viableCharacters.Contains(character))
            _viableCharacters.Remove(character);
    }

    public void StartGlobalCooldown()
    {
        _globalCooldown = GlobalBarkCooldown;
    }

    public bool GetCanPlayBarks()
    {
        return PlayerController.Instance.Input.GetIsInDefaultMap();
    }
}
