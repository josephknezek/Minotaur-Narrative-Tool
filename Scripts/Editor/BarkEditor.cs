using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BarkSO))]
public class BarkEditor : Editor
{
    private BarkSO myTarget;

    private SerializedProperty listenProperty;
    private SerializedProperty speakerProperty;
    private SerializedProperty priorityPadProperty;
    private SerializedProperty overrideProperty;
    private SerializedProperty conditionsProperty;
    private SerializedProperty onStartedProperty;
    private SerializedProperty responseProperty;
    private SerializedProperty displayTimeProperty;
    private SerializedProperty barkCooldownProperty;
    private SerializedProperty rememberProperty;
    private SerializedProperty onFinishedProperty;

    private GUIStyle timeSetStyle
    {
        get
        {
            if (_style != null)
                return _style;

            _style = new GUIStyle(EditorStyles.label);
            _style.fontStyle = FontStyle.Italic;

            return _style;
        }
    }

    private GUIStyle _style;

    private void OnEnable()
    {
        myTarget = (BarkSO)target;

        // Properties let's gooooooo
        listenProperty = serializedObject.FindProperty(nameof(myTarget.ListenEvent));
        speakerProperty = serializedObject.FindProperty(nameof(myTarget.Speaker));
        priorityPadProperty = serializedObject.FindProperty(nameof(myTarget.PriorityPad));
        overrideProperty = serializedObject.FindProperty(nameof(myTarget.OverrideCooldowns));
        conditionsProperty = serializedObject.FindProperty(nameof(myTarget.Conditions));
        onStartedProperty = serializedObject.FindProperty(nameof(myTarget.OnStartedEvents));
        responseProperty = serializedObject.FindProperty(nameof(myTarget.Response));
        displayTimeProperty = serializedObject.FindProperty(nameof(myTarget.TimeToDisplay));
        barkCooldownProperty = serializedObject.FindProperty(nameof(myTarget.BarkCooldown));
        rememberProperty = serializedObject.FindProperty(nameof(myTarget.OnRemember));
        onFinishedProperty = serializedObject.FindProperty(nameof(myTarget.OnFinishedEvents));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Show Advanced Settings?");
        myTarget.ShowAdvancedSettings = EditorGUILayout.Toggle(myTarget.ShowAdvancedSettings);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Show every field and sort with all context
        if (myTarget.ShowAdvancedSettings)
        {
            EditorGUILayout.LabelField("Bark Trigger Info", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(listenProperty, true);
            EditorGUILayout.PropertyField(speakerProperty, true);
            EditorGUILayout.PropertyField(priorityPadProperty, true);
            EditorGUILayout.PropertyField(overrideProperty, true);
            EditorGUILayout.PropertyField(conditionsProperty, true);
            EditorGUILayout.PropertyField(onStartedProperty, true);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Bark Content Info", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(responseProperty, true);
            DrawSuggestedTime();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Bark Completion Info", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(barkCooldownProperty, true);
            EditorGUILayout.PropertyField(rememberProperty, true);
            EditorGUILayout.PropertyField(onFinishedProperty, true);
        }
        else // Only show a small subset of properties
        {
            EditorGUILayout.LabelField("Bark Info", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(listenProperty, true);
            EditorGUILayout.PropertyField(speakerProperty, true);
            EditorGUILayout.PropertyField(conditionsProperty, true);
            EditorGUILayout.PropertyField(responseProperty, true);

            // Set the time to default
            myTarget.TimeToDisplay = myTarget._suggestedTime;
        }

        // Only set dirty and apply changes if there were changes
        if (EditorGUI.EndChangeCheck())
        {
            // Serialize!
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void DrawSuggestedTime()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(displayTimeProperty, true);
        EditorGUILayout.Space(5, false);

        if (GUILayout.Button($"Suggested: {myTarget._suggestedTime} sec", timeSetStyle))
        {
            myTarget.TimeToDisplay = myTarget._suggestedTime;
        }

        EditorGUILayout.EndHorizontal();
    }
}
