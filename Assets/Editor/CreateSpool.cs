using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateSpool : EditorWindow
{
    static string spoolName;

    public static void Init()
    {
        CreateSpool window = ScriptableObject.CreateInstance<CreateSpool>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowPopup();
        spoolName = "";
    }

    private void OnGUI()
    {
       spoolName = EditorGUILayout.TextField("Spool Name: ", spoolName);

        EditorGUILayout.BeginHorizontal("box");
        {
            if (GUILayout.Button("Create"))
            {
                CreateNew();
            }
            if (GUILayout.Button("Cancel"))
            {
                this.Close();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    void CreateNew()
    {
        Spool newSpool = new Spool();
        newSpool.name = spoolName;
        newSpool.stitchCollection = new Stitch[0];
        AssetDatabase.CreateFolder("Assets", spoolName);
        AssetDatabase.CreateAsset(newSpool, "Assets/" + spoolName + "/" + spoolName + ".asset");
        this.Close();
    }
}

