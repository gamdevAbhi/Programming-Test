using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObstacleManager))]
public class ObstacleEditor : Editor 
{
    SerializedProperty toggleData;
    SerializedProperty gridSize;
    SerializedProperty maxGrid;
    SerializedProperty isPressed;
    SerializedProperty takeData;
    SerializedProperty toggleChange;
    SerializedProperty playerIndex;
    SerializedProperty enemyIndex;
    
    void OnEnable()
    {
        toggleData = serializedObject.FindProperty("toggleData");
        gridSize = serializedObject.FindProperty("gridSize");
        maxGrid = serializedObject.FindProperty("maxGrid");
        isPressed = serializedObject.FindProperty("isPressed");
        takeData = serializedObject.FindProperty("takeData");
        toggleChange = serializedObject.FindProperty("toggleChange");
        playerIndex = serializedObject.FindProperty("playerIndex");
        enemyIndex = serializedObject.FindProperty("enemyIndex");

    }
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        int id = maxGrid.intValue - 1;

        List<bool> toggle = new List<bool>();

        for(int i = 0; i < maxGrid.intValue; i++)
        {
            toggle.Add(toggleData.GetArrayElementAtIndex(i).boolValue);
        }

        EditorGUILayout.BeginVertical();

        for(int y = gridSize.GetArrayElementAtIndex(1).intValue - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();

            for(int x = gridSize.GetArrayElementAtIndex(0).intValue - 1; x >= 0; x--)
            {
                if(id == enemyIndex.intValue)
                {
                    GUILayout.Label("E", GUILayout.ExpandWidth(true), GUILayout.Width(15), GUILayout.Height(15), 
                    GUILayout.MinWidth(5),GUILayout.MinHeight(5), GUILayout.MaxWidth(18));
                    toggleData.GetArrayElementAtIndex(id).boolValue = false;
                }
                else if(id == playerIndex.intValue)
                {
                    GUILayout.Label("P", GUILayout.ExpandWidth(true), GUILayout.Width(15), GUILayout.Height(15), 
                    GUILayout.MinWidth(5),GUILayout.MinHeight(5), GUILayout.MaxWidth(18));
                    toggleData.GetArrayElementAtIndex(id).boolValue = false;
                }
                else
                {
                    toggleData.GetArrayElementAtIndex(id).boolValue = EditorGUILayout.Toggle(toggle[id], GUILayout.Width(15), GUILayout.Height(15), 
                    GUILayout.MinWidth(5),GUILayout.MinHeight(5), GUILayout.MaxWidth(20));
                }
                
                id--;
            }

            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();

        string result = "";

        if(toggleChange.boolValue == true)
        {
            result = "Save Needed";
            isPressed.boolValue = GUILayout.Button("Store Data", GUILayout.ExpandWidth(true), GUILayout.Width(90), GUILayout.Height(20));
            takeData.boolValue = GUILayout.Button("Restore Data", GUILayout.ExpandWidth(true), GUILayout.Width(90), GUILayout.Height(20));
        }
        else
        {
            result = "Saved";
            takeData.boolValue = false;
            isPressed.boolValue = false;
        }

        GUILayout.Label(result, GUILayout.ExpandWidth(true), GUILayout.Width(120), GUILayout.Height(20));

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}