
using UnityEditor;

public static class ScriptGraphRepainter
{
    [MenuItem("Tools/Redraw Graph")]
    static void FixGraphDisplay()
    {
        EditorApplication.delayCall += EditorUtility.RequestScriptReload;
    }
}