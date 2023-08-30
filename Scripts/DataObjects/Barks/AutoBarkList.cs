
using UnityEngine;

[CreateAssetMenu(fileName = SingletonName, menuName = "Minotaur/Barks/Lists/New Auto List", order = 1)]
public class AutoBarkList : ManualBarkList 
{
    private const string SingletonName = "_AllBarks";

    public static AutoBarkList GlobalInstance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<AutoBarkList>(SingletonName);

            return _instance;
        }
    }
    private static AutoBarkList _instance;

    [HideInInspector, SerializeField]
    public string barksPath = "Assets";

    private void OnValidate()
    {
        if (!barksPath.StartsWith("Assets"))
        {
            Debug.LogWarning($"Bark path must include and start with \"Assets\".\n" +
                $"If you'd like to create a list that isn't autopopulated, use " +
                $"<b>Minotaur/Barks/Lists/Bark List</b>");
            barksPath = "Assets";
        }
    }
}
