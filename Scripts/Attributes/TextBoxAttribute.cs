using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxAttribute : PropertyAttribute 
{
    public int Lines;

    public TextBoxAttribute(int numLines = 2)
    {
        Lines = numLines;
    }
}
