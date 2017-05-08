using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class StitchNode : NodeBaseClass
{
    public Stitch MyStitch;

    public StitchNode(Rect r, int ID, Stitch stitch) : base(r, ID)
    {
        MyStitch = stitch;
    }

    public override void DrawGUI(int winID)
    {
        if (GUILayout.Button("Edit"))
        {
            StitchEditor editWindow = EditorWindow.GetWindow<StitchEditor>();

            editWindow.Init(MyStitch);
        }
        base.BaseDraw();
    }

    public override void AttachComplete(NodeBaseClass winID)
    {
        base.linkedNodes.Add(winID);
    }

}
