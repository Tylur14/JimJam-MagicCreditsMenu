using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CC_SectionUtil))]
public class CC_SectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CC_SectionUtil instance = (CC_SectionUtil)target;
        DrawDefaultInspector();

        GUILayout.BeginHorizontal();

        GUILayout.Label("Add Content Item", EditorStyles.boldLabel);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name:", EditorStyles.boldLabel);
        var newContentText = GUILayout.TextArea(instance.textBox, GUILayout.Width(150), GUILayout.Height(25));
        if (instance.textBox != newContentText.ToString())
            instance.textBox = newContentText.ToString();

        if (GUILayout.Button("Add Section", GUILayout.Width(150), GUILayout.Height(25)))
        {
            instance.AddContent(newContentText.ToString());
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

    }
}
