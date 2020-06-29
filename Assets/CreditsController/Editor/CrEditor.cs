using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreditsController))]
public class CrEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        CreditsController instance = (CreditsController)target;
        DrawDefaultInspector();

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Add Section",EditorStyles.boldLabel);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name:", EditorStyles.boldLabel);
        var newSectionText = GUILayout.TextArea(instance.textBox, GUILayout.Width(150), GUILayout.Height(25));
        if (instance.textBox != newSectionText.ToString())
            instance.textBox = newSectionText.ToString();

        if (GUILayout.Button("Add Section", GUILayout.Width(150), GUILayout.Height(25)))
        {
            instance.AddSection(newSectionText.ToString());
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Reload", GUILayout.Width(150), GUILayout.Height(25)))
        {
            instance.ReloadItems();
        }
    }
}
