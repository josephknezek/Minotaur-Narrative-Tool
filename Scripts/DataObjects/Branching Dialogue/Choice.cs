using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

[Inspectable]
public class Choice
{
    public List<string> ChoiceTexts;   // The list of choice texts

    public Choice(List<string> choices)
    {
        ChoiceTexts = choices;
    }
}
