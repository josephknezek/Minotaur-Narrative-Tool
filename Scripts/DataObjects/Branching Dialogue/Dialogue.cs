using Unity.VisualScripting;
using System;

[Serializable, Inspectable]
public class Dialogue
{
    public int LineID;              // The ID of the dialogue line
    public Character Character;     // The character speaking the dialogue
    public string LineText;         // The text of the dialogue
    public string ChoiceText;       // Optional text for the dialogue choice

    public Dialogue(int id, Character character, string lineText, string choice = null)
    {
        LineID = id;
        Character = character;
        LineText = lineText;
        ChoiceText = choice;
    }
}
