using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;
using System.Linq;

[CustomEditor(typeof(AutoBarkList))]
public class AutoBarksListEditor : Editor
{
    private AutoBarkList list;

    private SerializedProperty barkPathProperty;

    private void OnEnable()
    {
        list = (AutoBarkList)target;

        barkPathProperty = serializedObject.FindProperty(nameof(list.barksPath));
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(barkPathProperty);
        if (GUILayout.Button("Manually Retrieve/Confirm Barks"))
        {
            if (!Directory.Exists(list.barksPath))
            {
                Debug.LogError($"Path \"{list.barksPath}\" is not a valid directory!");
                base.OnInspectorGUI();
                return;
            }

            RepopulateBarksList();
        }

        // Only set dirty and apply changes if there were changes
        if (EditorGUI.EndChangeCheck())
        {
            // Serialize!
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.Space();

        base.OnInspectorGUI();
    }

    private void RepopulateBarksList()
    {
        list.BarkRules.Clear();

        GetBarksInDirectoryRecursive(list.barksPath);
    }

    private void GetBarksInDirectoryRecursive(string path)
    {
        List<BarkSO> barks = AssetDatabaseExtensions.GetAssetsOfTypeRecursivelyFromPath<BarkSO>(path);

        foreach (var bark in barks)
        {
            if (list != AutoBarkList.GlobalInstance)
            {
                bark.RegisterToBarkList(AssetDatabase.GetAssetPath(list));
            }
            else
            {
                bark.RegisterToBarkList();
            }
        }
    }


}
