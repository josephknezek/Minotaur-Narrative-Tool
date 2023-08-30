
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public static class AssetDatabaseExtensions
{

    public static string GetDirectoryPathFromFilePath(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = "Assets";
        }
        else if (Path.GetExtension(filePath) != "")
        {
            filePath = filePath.Replace(Path.GetFileName(filePath), "");
        }

        return filePath;
    }


    /// <summary>
    /// Gets all assets of a given type from a base path regardless of their folder nesting.
    /// </summary>
    /// <typeparam name="T">The type of asset to gather.</typeparam>
    /// <param name="baseDirectory">The base directory to start the search from.</param>
    /// <returns>A list of all found assets of type T.</returns>
    public static List<T> GetAssetsOfTypeRecursivelyFromPath<T>(string baseDirectory) where T : class
    {
        List<T> objects = new();

        // Iterate through each directory recursively
        foreach (var dirPath in Directory.GetDirectories(baseDirectory))
        {
            string local = ReplaceStupid(dirPath);

            objects.AddRange(GetAssetsOfTypeRecursivelyFromPath<T>(local));
        }

        // Actually get the files we're looking for
        foreach (var filePath in Directory.GetFiles(baseDirectory))
        {
            // Ignore .meta files and any other random nonsense.
            if (Path.GetExtension(filePath) != ".asset")
                continue;

            string local = ReplaceStupid(filePath);

            if (AssetDatabase.LoadMainAssetAtPath(local) is T obj)
                objects.Add(obj);
        }

        return objects;
    }


    private static string ReplaceStupid(string dumb)
    {
        // Friggen Directory.GetDirectories returning a backslash where the search
        // was initialized from >:(
        return dumb.Replace('\\', '/');
    }
}
#endif