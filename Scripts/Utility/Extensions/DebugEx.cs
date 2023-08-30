using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugEx
{

    public static void DrawBounds(Bounds bounds)
    {
        DrawBounds(bounds, Color.red);
    }

    public static void DrawBounds(Bounds bounds, Color color)
    {
        bounds = new Bounds(bounds.center, new Vector3(bounds.size.x, bounds.size.y, 0f));

        Debug.DrawLine(bounds.min, new Vector3(bounds.min.x, bounds.max.y, bounds.min.z), color);
        Debug.DrawLine(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z), color);
        Debug.DrawLine(bounds.max, new Vector3(bounds.max.x, bounds.min.y, bounds.min.z), color);
        Debug.DrawLine(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z), new Vector3(bounds.min.x, bounds.min.y, bounds.min.z), color);
    }

}
