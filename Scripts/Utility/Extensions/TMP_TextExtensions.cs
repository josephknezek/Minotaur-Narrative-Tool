using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class TMP_TextExtensions
{
    
    public static Bounds GetWorldspaceTextBounds(this TMP_Text text)
    {
        return GetWorldspaceTextBounds(text, Vector2.zero);
    }

    public static Bounds GetWorldspaceTextBounds(this TMP_Text text, Vector2 padding)
    {
        return new Bounds(text.transform.position, text.textBounds.size + new Vector3(padding.x, padding.y));
    }

}
