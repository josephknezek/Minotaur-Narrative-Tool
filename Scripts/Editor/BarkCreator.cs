using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class BarkCreator
{

    // ChatGPT is a godsend, but if anyone reading this knows how to make it
    // better please message me about it
    [MenuItem("Assets/Create/Minotaur/Barks/New Bark", false, -1)]
    public static void CreateNewBark()
    {
        BarkSO newObj = ScriptableObject.CreateInstance<BarkSO>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        path = AssetDatabaseExtensions.GetDirectoryPathFromFilePath(path);

        // If the AllBarksList instance STARTS with the path, add it. (Allows recursion)
        if (path.StartsWith(AutoBarkList.GlobalInstance.barksPath))
            newObj.RegisterToBarkList();

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New Bark.asset");

        AssetDatabase.CreateAsset(newObj, assetPathAndName);
        EditorUtility.SetDirty(newObj);
        AssetDatabase.SaveAssets();

        EditorApplication.delayCall += AssetDatabase.Refresh;
        EditorApplication.delayCall += () =>
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newObj;

            // Simulate default SO creation behavior
            EditorApplication.update += EnterRenameState;
        };
    }

    private static void EnterRenameState()
    {
        if (Selection.activeObject != null)
        {
            EditorApplication.update -= EnterRenameState;
            EditorWindow projectWindow = EditorWindow.focusedWindow;
            if (projectWindow != null && projectWindow.GetType().Name == "ProjectBrowser")
            {
                EditorApplication.ExecuteMenuItem("Assets/Rename");
            }
        }
    }
}
