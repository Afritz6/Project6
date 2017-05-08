using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeEditor : EditorWindow
{
    public List<NodeBaseClass> myNode = new List<NodeBaseClass>();
    public Spool spool;
    public Yarn yarn;

    public Texture myTexture;
    public GUISkin mySkin;

    public int nodeAttachId;

    [MenuItem("Node Editor/ Node Editor")]

    
    public static void showWindow()
    {
        GetWindow<NodeEditor>();
    }

    void OnEnable()
    {
        myTexture = (Texture)EditorGUIUtility.Load("Blur.jpg");
        mySkin = (GUISkin)EditorGUIUtility.Load("MS.guiskin");
    }

    public void OnGUI()
    {
        GUI.skin = mySkin;
        GUI.DrawTexture(new Rect(0, 0, position.width, position.height), myTexture);
        if (GUI.Button(new Rect(position.width - 130, 20 , 125, 50), "Create New Story"))
        {
            CreateSpool.Init();
        }

        if (GUI.Button(new Rect(position.width - 130, 75, 125, 50), "Add Stitch"))
        {
            Debug.Log("Must have a spool selected");
            if (spool != null)
            {
                int ID = myNode.Count;
                Stitch tryingNewStitch = new Stitch();
                tryingNewStitch.stitchID = ID;
                int nameID = ID + 1;
                tryingNewStitch.stitchName = "Stitch" + nameID;
                tryingNewStitch.yarns = new Yarn[1];
                AssetDatabase.CreateAsset(tryingNewStitch, "Assets/" + spool.name + "/" + tryingNewStitch.stitchName + ".asset");
                myNode.Add(new StitchNode(new Rect(10, 20, 100, 100), myNode.Count - 1, tryingNewStitch));
                List<Stitch> tempList = new List<Stitch>();
                foreach (StitchNode node in myNode)
                {
                    tempList.Add(node.MyStitch);
                }
                spool.stitchCollection = tempList.ToArray();
            }
        }

        EditorGUI.BeginChangeCheck();
        spool = (Spool)EditorGUILayout.ObjectField(spool, typeof(Spool), false);
        if (EditorGUI.EndChangeCheck())
        {
            if (spool != null)
            {
                myNode.Clear();
                for (int i = 0; i < spool.stitchCollection.Length; i++)
                {
                    myNode.Add(new StitchNode(new Rect(i * 110, 20, 100, 100), i, spool.stitchCollection[i]));
                    myNode[i].title = spool.stitchCollection[i].name;
                }
            }
        }

        if (spool != null)
        {
            for (int i = 0; i < spool.stitchCollection.Length; i++)
            {
                for (int j = 0; j < spool.stitchCollection[i].yarns.Length; j++)
                {
                    if (spool.stitchCollection[i].yarns[j].choiceStitch != null)
                    {
                        DrawNodeCurve(myNode[i].rect, myNode[spool.stitchCollection[i].yarns[j].choiceStitch.stitchID].rect);
                    }
                }
            }
        }
        BeginWindows();
        for (int i = 0; i < myNode.Count; i++)
        {
            myNode[i].rect = GUI.Window(i, myNode[i].rect, myNode[i].DrawGUI, spool.stitchCollection[i].stitchName);
        }

        EndWindows();
    }

    private void OnDestroy()
    {
    //    SaveLayout();
    }

    public void RemoveNode(int id)
    {
        //broken
        for (int i = 0; i < myNode.Count; i++)
        {
            for (int j = 0; j < spool.stitchCollection[i].yarns.Length; j++)
            {
                if (spool.stitchCollection[i].yarns[j].choiceStitch != null)
                {
                    myNode[i].RemoveStitch(spool, spool.stitchCollection[i]);
                    myNode[i].ReassignID(i);
                }
            }
            myNode[i].linkedNodes.RemoveAll(item => item.id == id);
        }
        myNode.RemoveAt(id);
        UpdateNodeIDs();
    }

    public void UpdateNodeIDs()
    {
        //broken
        for (int i = 0; i < myNode.Count; i++)
        {
            for (int j = 0; j < spool.stitchCollection[i].yarns.Length; j++)
            {
                if (spool.stitchCollection[i].yarns[j].choiceStitch != null)
                {
                    myNode[i].ReassignID(i);
                }
            }

        }
    }

    public void BeginAttachment(int winID)
    {

        yarn.choiceStitch.stitchID = winID;
    }

    public void EndAttachment(int winID)
    {
        if (yarn.choiceStitch.stitchID > -1)
        {
            myNode[yarn.choiceStitch.stitchID].AttachComplete(myNode[winID]);
        }
        yarn.choiceStitch.stitchID = -1;
    }

    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + (start.height / 2) + 10, 0);
        Vector3 endPos = new Vector3(end.x, end.y + (end.height / 2) + 10, 0);
        Vector3 startTan = startPos + Vector3.right * 100;
        Vector3 endTan = endPos + Vector3.left * 100;
        Color shadowCol = new Color(0, 0, 0, 0.06f);


        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 5);
    }
}
