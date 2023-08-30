
using UnityEngine;

public class LabelAsAttribute : PropertyAttribute
{
    public string LabelOverride = "";

    public LabelAsAttribute(string label)
    {
        LabelOverride = label;
    }

}
