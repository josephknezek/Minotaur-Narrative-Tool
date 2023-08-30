
using UnityEngine;
using UnityEditor;
using System.IO;

// The internet knows more than me.
// Link: https://docs.unity3d.com/ScriptReference/AssetModificationProcessor.html
public class BarkGlobalRegistrationInterceptor : AssetModificationProcessor
{
    static void OnWillCreateAsset(string assetName)
    {
        if (Path.GetExtension(assetName) == ".meta" || Selection.activeObject == null)
            return;

        // Minor redundancy. Doesn't hurt
        if (Selection.activeObject.GetType() == typeof(BarkSO))
        {
            EditorApplication.delayCall += () => RegisterBarkOfName(assetName);
        }
    }

    static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions opt)
    {
        if (Path.GetExtension(path) == ".meta")
            return AssetDeleteResult.DidNotDelete;

        if (CheckAndDeleteBark(path))
            return AssetDeleteResult.DidNotDelete;

        if (Directory.Exists(path))
        {
            DeleteDirectoriesRecursive(path);
        }

        return AssetDeleteResult.DidNotDelete;
    }


    private static void RegisterBarkOfName(string pathWithName)
    {
        if (!pathWithName.StartsWith(AutoBarkList.GlobalInstance.barksPath))
            return;

        BarkSO bark = AssetDatabase.LoadMainAssetAtPath(pathWithName) as BarkSO;

        if (bark != null)
        {
            bark.RegisterToBarkList();
        }
    }

    public static bool CheckAndDeleteBark(string path)
    {
        if (AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(BarkSO))
        {
            BarkSO deleted = (BarkSO)AssetDatabase.LoadAssetAtPath(path, typeof(BarkSO));
            deleted.UnregisterFromGlobal();
            EditorUtility.SetDirty(AutoBarkList.GlobalInstance);
            AssetDatabase.SaveAssets();
            return true;
        }

        return false;
    }

    private static void DeleteDirectoriesRecursive(string path)
    {
        // Get all directories recursively
        foreach (var dirPath in Directory.GetDirectories(path))
        {
            string localPath = dirPath.Replace('\\', '/');

            DeleteDirectoriesRecursive(localPath);
        }

        // Look for barks
        foreach (var filePath in Directory.GetFiles(path))
        {
            if (Path.GetExtension(filePath) == ".meta")
                continue;

            // Why
            string localPath = filePath.Replace('\\', '/');

            CheckAndDeleteBark(localPath);
        }
    }
}