
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event Holder", menuName = "Minotaur/Barks/Utility/New Event Holder", order = 2)]
public class BarkEventHolder : ScriptableObject
{
    [SerializeField]
    private List<BarkSOEvent> _barkEvents = new List<BarkSOEvent>();

    public BasicSOEvent GetBarkEvent(string name)
    {
        return _barkEvents.Find(x => x.name == name);
    }
}
