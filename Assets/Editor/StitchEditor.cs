using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[System.Serializable]
public class StitchEditor : EditorWindow
{
    public Stitch myStitch;

    public void Init(Stitch stitch)
    {
        myStitch = stitch;
    }

    private void OnGUI()
    {
        myStitch.stitchName = EditorGUILayout.TextField("Stitch Name", myStitch.stitchName);

        myStitch.summary = EditorGUILayout.TextField("Summary", myStitch.summary);

        myStitch.background = (Sprite)EditorGUILayout.ObjectField("Background", myStitch.background, typeof(Sprite), true) as Sprite;

        ScriptableObject stitch = myStitch;
        SerializedObject stitchInfo = new SerializedObject(stitch);

        SerializedProperty performers = stitchInfo.FindProperty("performers");
        EditorGUILayout.PropertyField(performers, true);
        stitchInfo.ApplyModifiedProperties();

        SerializedProperty dialogs = stitchInfo.FindProperty("dialogs");
        EditorGUILayout.PropertyField(dialogs, true);
        stitchInfo.ApplyModifiedProperties();

        SerializedProperty yarns = stitchInfo.FindProperty("yarns");
        EditorGUILayout.PropertyField(yarns, true);
        stitchInfo.ApplyModifiedProperties();

        myStitch.status = (Stitch.stitchStatus)EditorGUILayout.EnumPopup("Status", myStitch.status);

    }
}

